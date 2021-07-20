using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Snake[] snake = new Snake[150];

    Board board;
    float tickTime;
    float speed = 0.08f;
    int length = 2; //몸통길이
    Vector3 dir = Vector3.down;
   [SerializeField] bool isEat = false;
    void Start()
    {
        for (int i = 0; i < 150; i++)
        {
            snake[i] = new Snake();
        }

        board = FindObjectOfType<Board>();
        board.CreateMap();

        CreateSnake();
    }

    private void Update()
    {
        ChangeSnakeDir();
        tickTime += 1 * Time.deltaTime;
        if (tickTime >= speed)
        {
            tickTime = 0;
            MoveSnake();
        }
        if(isEat)
        {
           if(board.CreateApple())
            {
                isEat = false;
            }
        }
    }
    void Dead()
    {
        dir = Vector3.down;
        length = 2;
        tickTime = 0;

        for(int i =0;i<snake.Length;i++)
        {
            snake[i].x = 0;
            snake[i].y = 0;
        }

        CreateSnake();
    }
    void MoveSnake()
    {
        for (int i = length; i > 0; i--)
        {
            snake[i].x = snake[i - 1].x;
            snake[i].y = snake[i - 1].y;
        }

        snake[0].x += (int)dir.x;
        snake[0].y += (int)dir.y;

        if (board.CheckEatApple(snake[0].x, snake[0].y))
        {
            length++;
            isEat = true;
            if (length % 3 == 0) board.CreateWall();
        }

        if (snake[0].x <= 0 || snake[0].y <= 0 || snake[0].x >= board.maxSize - 1 || snake[0].y >= board.maxSize - 1 || board.Grid[snake[0].y,snake[0].x] == (int)CellType.WALL || board.Grid[snake[0].y, snake[0].x] == (int)CellType.SNAKE)
        {
            board.ResetBoard();
            Dead();        
        }

        for (int i = 0; i < length; i++)
        {
            board.ChangeGridValue(snake[i].x, snake[i].y, CellType.SNAKE);
        }
        board.ChangeGridValue(snake[length].x, snake[length].y, CellType.EMPTY);
    }

    void ChangeSnakeDir()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && dir != Vector3.right)
        {
            dir = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && dir != Vector3.left)
        {
            dir = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && dir != Vector3.down)
        {
            dir = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && dir != Vector3.up)
        {
            dir = Vector3.down;
        }
    }
    void CreateSnake()
    {
        int x = Random.Range(1, board.maxSize - 1);
        int y = Random.Range(board.maxSize / 2, board.maxSize - 1);

        snake[0].x = x;
        snake[0].y = y;

        board.ChangeGridValue(x, y, CellType.SNAKE);
        board.CreateApple();
    }

}
