using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Level : MonoBehaviour
{
    private const float PipeWidth = 8f;
    private const float PipeHeadHeight = 3.75f;
    private const float CameraOrthoSize = 50f;
    private List<Pipe> pipelist;
    private List<Pipe> pipelistScore;
    private const float MovementSpeed = 30f;
    private const float Pipedestroypos = -105f;
    private const float Pipespawnpos = +105f;
    private const float birdPos = 0f;
    private static Level Instance;
    private int pipesSpawned;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapsize;
    private int scorePoints;
    private State state;

    public static Level GetInstance()
    {
        return Instance;
    }
     
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead,
    }

    private void Awake()
    {
        Instance = this;
        pipelist = new List<Pipe>();
        pipelistScore = new List<Pipe>();
        pipeSpawnTimerMax = 1f;
        SetDifficulty(Difficulty.Easy);
        scorePoints = 0;
        state = State.WaitingToStart;
    }
    private void Start()
    {
        Bird.GetInstance().onDied += Bird_onDied;
        Bird.GetInstance().onStartedPlaying += Bird_onStartedPlaying;
    }

    private void Bird_onStartedPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }

    private void Bird_onDied(object sender,System.EventArgs e)
    {
        state = State.BirdDead;
    }

    private void Update()
    {
        if(state == State.Playing)
        {
            handlePipeMovement();
            handlePipeSpawnning();
        }
        
    }

    private void handlePipeSpawnning()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if(pipeSpawnTimer < 0)
        {
            pipeSpawnTimer += pipeSpawnTimerMax;

            float heightedgelimit = 10f;
            float minheight = gapsize * .5f + heightedgelimit;
            float toatlheight = CameraOrthoSize * 2f;
            float maxheight = toatlheight - gapsize * .5f - heightedgelimit;
            float height = Random.Range(minheight, maxheight);
            creategap(height, gapsize, Pipespawnpos);
        }
    }

    private void Score()
    {
        for(int i = 0; i < pipelistScore.Count; i++)
        {
            Pipe pipe = pipelistScore[i];
            if(pipe.Getpipepos() < birdPos)
            {
                scorePoints++;
                SoundManager.PlaySound(SoundManager.Sound.Score);
                pipelistScore.Remove(pipe);
                i--;
            }
        }
    }

    public int getScore()
    {
        return scorePoints / 2;
    }

    private void handlePipeMovement()
    {
        for (int i = 0; i < pipelist.Count; i++)
        {
            Pipe pipe = pipelist[i];
            pipe.Move();
            if(pipe.Getpipepos() < Pipedestroypos)
            {
                pipe.DestroySelf();
                pipelist.Remove(pipe);
                i--;
            }
        }
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapsize = 50f;
                pipeSpawnTimerMax = 1.2f;
                break;
            case Difficulty.Medium:
                gapsize = 40f;
                pipeSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Hard:
                gapsize = 33;
                pipeSpawnTimerMax = 1f;
                break;
            case Difficulty.Impossible:
                gapsize = 24f;
                pipeSpawnTimerMax = .8f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 50) return Difficulty.Impossible;
        if (pipesSpawned >= 30) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private void creategap(float gapY, float gapsize, float gappos)
    {
        CreatePipe(gapY - gapsize * .5f, gappos, true);
        CreatePipe(CameraOrthoSize * 2 - gapY - gapsize * .5f, gappos, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
        Score();
    }
    private void CreatePipe(float height, float xpos, bool createbottom)
    {
        //create pipe head
        Transform pipehead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeheadpos;
        if (createbottom)
        {
            pipeheadpos = -CameraOrthoSize + height - PipeHeadHeight / 2 * .5f;
        }
        else
        {
            pipeheadpos = +CameraOrthoSize - height + PipeHeadHeight / 2 * .5f;
        }
        pipehead.position = new Vector3(xpos, pipeheadpos);

        //create pipe body
        Transform pipebody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        float pipebodypos;
        if (createbottom)
        {
            pipebodypos = -CameraOrthoSize;
        }
        else
        {
            pipebodypos = +CameraOrthoSize;
            pipebody.localScale = new Vector3(1, -1, 1);
        }
        pipebody.position = new Vector3(xpos, pipebodypos);

        SpriteRenderer pipebodyspriterenderer = pipebody.GetComponent<SpriteRenderer>();

        pipebodyspriterenderer.size = new Vector2(PipeWidth, height);
        BoxCollider2D pipeBodyBoxCollider = pipebody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PipeWidth, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * 0.5f);

        Pipe pipe = new Pipe(pipehead, pipebody);
        pipelist.Add(pipe);
        pipelistScore.Add(pipe);
    }

    public int GetPipesSpawned()
    {
        return pipesSpawned;
    }

    private class Pipe
    {
        private Transform pipeheadtransform;
        private Transform pipebodytransform;

        public Pipe(Transform pipeheadtransform, Transform pipebodytransform)
        {
            this.pipebodytransform = pipebodytransform;
            this.pipeheadtransform = pipeheadtransform;
        }

        public void Move()
        {
            pipebodytransform.position += new Vector3(-1, 0, 0) * MovementSpeed * Time.deltaTime;
            pipeheadtransform.position += new Vector3(-1, 0, 0) * MovementSpeed * Time.deltaTime;
        }

        public float Getpipepos()
        {
            return pipebodytransform.position.x;
        }

        public void DestroySelf()
        {
            Destroy(pipebodytransform.gameObject);
            Destroy(pipeheadtransform.gameObject);
        }
    }
}
