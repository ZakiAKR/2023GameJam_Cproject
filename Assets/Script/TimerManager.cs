using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

// 時間に関するソースコード

public class TimerManager : MonoBehaviour
{
    // カウントダウンを表示させるtextを取得
    [SerializeField] private TextMeshProUGUI countDownText;
    // カウントダウンを表示させるTextのオブジェクトを取得
    [SerializeField] private GameObject countObj;

    [Space(10)]

    // 全体の時間を表示させるtextを取得
    [SerializeField] private TextMeshProUGUI lifeText;

    [Space(10)]

    // 終了を表示させるtextを取得
    [SerializeField] private TextMeshProUGUI overText;
    // 終了を表示させるtextのオブジェクトを取得
    [SerializeField] private GameObject overObj;

    [Space(10)]

    // 一問分の時間の文字を表示するtextを取得
    [SerializeField] private TextMeshProUGUI mondaiTimeText;

    [Space(10)]

    // シーン遷移をするために「TranstionManager」を取得
    [SerializeField] public TranstionManager transScene;

    // メインのシステムの関数を使用するため、「TypingManager」を取得
    [SerializeField] public TypingManager typeSystem;

    [Space(10)]

    // Startの表示時間
    public float startWaitTime;
    // Finishの表示時間
    public float finishWaitTime;

    [Space(10)]

    // Startに入る際の演出が終わったかどうかの判定
    [HideInInspector]public bool isCountDown;

    // 全体の時間が終わったかどうかの判定
    [HideInInspector]public bool isFinish;

    // 時間計測する用の変数
    private float _countDown = 3f;
    // カウントダウンの数字を表示するための変数
    private int _count;

    [Space(10)]

    // 全体の時間を計測
    public float lifeTime;
    // 全体の時間の数字を表示するための変数
    private int _life;

    [Space(10)]

    // 一問分の時間の値を保持するための変数
    public static float typeTime = 8;
    // 一問分の時間を測定するための変数
    [HideInInspector] public static float _typeTime;
    // 一問分の時間の値をtextで表示するための変数
    private int _type;


    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        countObj.SetActive(true);
        overObj.SetActive(false);
        _count = 0;
        _life = 0;
        isCountDown = false;
        isFinish = false;
        _typeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // カウントダウンの処理
        if (_countDown >= 0)
        {
            // 時間を計測して残り時間を表示する処理
            // 時間を減らす
            _countDown -= Time.deltaTime;

            // textに表示するためにint型に変換
            _count = (int)_countDown + 1;

            // 残り時間を表示
            countDownText.text = _count.ToString();
        }
        // _isCountDownの判定を付けるのは「Start」が表示されている時に全体の時間が計測されないようにするため
        if (!isCountDown && _countDown <= 0)
        {
            // Startを表示するためのコルーチン
            StartCoroutine(Delay_StartText());
        }

        // 全体の時間の処理
        // _isFinishの判定を付けているのは「Finish」が表示されているときに全体の時間が計測されないようにするため
        if (isCountDown && !isFinish)
        {
            // 時間を計測して残り時間を表示する処理
            // 時間を減らす
            lifeTime -= Time.deltaTime;

            // textに表示するためにint型に変換
            _life = (int)lifeTime;

            // 残り時間を表示
            lifeText.text = _life.ToString();
        }
        if (lifeTime <= 0)
        {
            // 全体の時間が終了した
            isFinish = true;

            // Finishを表示するためのコルーチン
            StartCoroutine(Delay_OverText());
        }

        // カウントダウン後でかつ、全体の時間内での処理をさせる
        if (isCountDown && !isFinish)
        {
            if (_typeTime > 0)
            {
                // 時間を計測して残り時間を表示する処理
                // 時間を減らす
                _typeTime -= Time.deltaTime;

                // textに表示するためにint型に変換
                _type = (int)_typeTime;

                // 残り時間を表示
                mondaiTimeText.text = _type.ToString();
            }
            if (_typeTime <= 0)
            {
                // 問題の初期化の関数を呼び出し
                typeSystem.Initi_Question();
            }
        }

    }

    // Startを表示するためのコルーチン
    private IEnumerator Delay_StartText()
    {
        // textに「Start」を表示
        countDownText.text = "START!!";

        // textを一定時間表示したままにする
        yield return new WaitForSeconds(startWaitTime);

        // textのオブジェクトを非表示にする
        countObj.SetActive(false);

        // カウントダウンが終了した
        isCountDown = true;
    }

    // Finishを表示するためのコルーチン
    private IEnumerator Delay_OverText()
    {
        // textのオブジェクトを表示にする
        overObj.SetActive(true);

        // textに「Finish」を表示
        overText.text = "FINISH!!";

        // textを一定時間表示したままにする
        yield return new WaitForSeconds(finishWaitTime);

        // リザルト画面を遷移する
        transScene.To_Result();
    }
}
