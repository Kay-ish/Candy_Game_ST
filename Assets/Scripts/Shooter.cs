using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    const int MaxShotPower = 5; //マックスで連続投入できる数 5という設定
    const int RecoverySeconds = 3; //回復までタイム

    int shotPower = MaxShotPower; //MaxShotPowerで設定した数がそのままshotパワーになっている

    AudioSource shotSound;

    //複数パターンモデルからランダムで出したいので配列する
    //public GameObject candyPrefab; //Instantiateで生成する対象
    public GameObject[] candyPrefabs; //Instantiateで生成する対象（配列）

    public Transform candyParentTransform; //生成されたCandyの親役
    public CandyManager candyManager; //CandyManagerクラスの変数を使えるようにする


    public float shotForce; //AddForceで使うパワー
    public float shotTorque; //AddTorqueで使う回転力

    public float baseWidth; //Candyが飛んでいく横の幅の上限 "5"の幅をめがけて飛んでいく



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Screen.width:" + Screen.width);

        //このscriptがついてるOBJからGetCompoしてshotSoundに代入
        //これで　shotSound.Play();　するだけで再生する
        shotSound = this.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Shot();
        //}

        //特定のボタンが押された時にShot()メソッドを発動
        if (Input.GetButtonDown("Fire1")) Shot();
    }

    //実行した結果　戻り値は　voidじゃなくGameObjectが返ってくるのでGameObject
    //配列candyPrefabsの中からランダムにオブジェクトを1個取り出すメソッド
    GameObject sampleCandy()
    {

        //通常はRandom.Range(0, 10)レンジは0～10とかいう指定、今回は.Lengthを使ってPrefabsの数という指定
        int index = Random.Range(0, candyPrefabs.Length);
        return candyPrefabs[index];
    }

    //実行した結果　void　じゃなくVector3が返ってくるから頭 Vector3
    //マウスが押された位置と連動するようにBaseのどこをめがけてCandyを飛ばすか、
    //その位置を決めている
    Vector3 GetInstantiatePosition()
    {

        //Input.mousePosition.x→Gameビュー上のマウスのx座標
        float xx = Input.mousePosition.x;
        Debug.Log(xx);

        //(baseWidth / 2)を引いてるのは計算の基準基点がずれてる分の補正
        float x = baseWidth * (xx / Screen.width) - (baseWidth / 2);
        return transform.position + new Vector3(x, 0, 0);
    }

    public void Shot()
    {
        //指定した数を使い切って残数0なら何もやらずにメソッド終了※if文 省略形記述
        if (candyManager.GetCandyAmount() <= 0) return; 

        if (shotPower <= 0) return;


        //①Candyの生成 Instantiate(対象物,位置,回転)
        //GameObject candy = Instantiate(
        //    candyPrefab,
        //    transform.position,
        //    Quaternion.identity
        //    );

        GameObject candy = Instantiate(
            sampleCandy(),
            GetInstantiatePosition(),
            Quaternion.identity
            );

        //生成したcandyオブジェクトの親は = candyParentTransform変数に指定したオブジェクト(Candies)
        //親子を勝手に組む指定！
        candy.transform.parent = candyParentTransform;


        //②生成したCandyのRigidbodyを使えるようにしている
        Rigidbody candyRigidBody = candy.GetComponent<Rigidbody>();

        //③生成したCandyにAddForce()メソッドをかけて飛ばしている
        //transform.forward→オブジェクトの前方
        candyRigidBody.AddForce(transform.forward * shotForce);
        
        //④横にスピンさせる力
        candyRigidBody.AddTorque(new Vector3(0, shotTorque, 0));    //(x,y,z)→yがトルクってる状態に

        //Candyのストックを消費
        candyManager.ConsumeCandy();

        ConsumePower();

        shotSound.Play(); //ShooterのAudioSourceに設置されているAudioClipを再生する
    }

    void OnGUI()
    {
        GUI.color = Color.black;

        string label = "";
        for (int i = 0; i < shotPower; i++) label = label + "+";

        GUI.Label(new Rect(50, 65, 100, 30), label);
    }


    //シューターのパワー増減
    void ConsumePower()
    {
        shotPower--;　//パワーを1減らす
        StartCoroutine(RecoverPower());　//コルーチン（回復開始）
    }

    IEnumerator RecoverPower()
    {
        yield return new WaitForSeconds(RecoverySeconds);
        shotPower++;
    }
}
