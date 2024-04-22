using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 他のクラスから使用するGameManagerインスタンス
    public static GameManager Instance { get; private set; }
    // 次のフルーツを生成可能かどうか
    // Fruit.Drop()でtrueにする
    public bool isNext { get; set; }
    // フルーツの種類数
    public int fruitLength { get; private set; }

    [SerializeField] private GameObject[] Fruits;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private TextMeshProUGUI ResultScoreText;

    private int score;

    // ボールの生成関連の定数
    private const float GEN_X_POS = 0.0f;
    private const float GEN_Y_POS = 3.5f;
    private const float GEN_INTERVAL = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        isNext = false;
        fruitLength = Fruits.Length;
        score = 0;
        SetScore();
        GenerateFruit();
    }

    // Update is called once per frame
    void Update()
    {
        // 次のフルーツを生成可能になったら2秒後に生成する
        if (isNext)
        {
            isNext = false;
            Invoke("GenerateFruit", GEN_INTERVAL);
        }
    }

    // フルーツを生成する
    private void GenerateFruit()
    {
        // 生成するフルーツのインデックス
        int fruitIdx = Random.Range(0, fruitLength - 4);
        Fruit fruit = Instantiate(Fruits[fruitIdx],
            new Vector2(GEN_X_POS, GEN_Y_POS),
            Quaternion.identity).GetComponent<Fruit>();
        fruit.id = fruitIdx;
    }

    // フルーツを合体する（ひとつ大きいフルーツを生成する）
    public void MergeFruits(Vector3 genPos, int parentId)
    {
        // 一番大きいフルーツだったら何も生成しない
        if (parentId == fruitLength - 1)
        {
            return;
        }
        // 引数に与えられた座標にひとつ大きいフルーツを生成する
        Fruit newFruit = Instantiate(Fruits[parentId + 1], genPos,
            Quaternion.identity).GetComponent<Fruit>();
        newFruit.id = parentId + 1;
        newFruit.hasDropped = true;
        newFruit.GetComponent<Rigidbody2D>().simulated = true;
        CalcScore(parentId);
    }

    // スコアをUIテキストにセットする
    private void SetScore()
    {
        ScoreText.text = score.ToString();
    }

    // スコアを計算する
    private void CalcScore(int fruitId)
    {
        score += (int)Mathf.Pow(3, fruitId);
        SetScore();
    }

    // ゲームオーバー処理
    public void GameOver()
    {
        ResultScoreText.text = "SCORE: " + score.ToString();
        GameOverPanel.SetActive(true);
    }
}
