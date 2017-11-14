using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnityChanController : MonoBehaviour {
	//アニメーションのためのコンポーネント
	private Animator myAnimator;
	//移動させるためのコンポーネント
	private Rigidbody myRigidbody;
	//前進するための力
	private float forwardForce=800.0f;
	//左右に移動するための力
	private float turnForce=500.0f;
	//ジャンプするための力
	private float upForce=500.0f;
	//左右の移動できる範囲
	private float movableRange=3.4f;
	//減速させる係数
	private float coefficient=0.95f;
	//ゲーム終了の判定
	private bool isEnd=false;
	//ゲーム終了時に表示するテキスト
	private GameObject stateText;
	//スコアを表示するテキスト
	private GameObject scoreText;
	//スコア
	private int score=0;
	//ボタン押下の判定
	private bool isLButtonDown=false;
	private bool isRButtonDown = false;


	// Use this for initialization
	void Start () {
		//コンポーネントを取得
		this.myAnimator=GetComponent<Animator>();
		//走るアニメーションを開始
		this.myAnimator.SetFloat("Speed",1);
		//コンポーネントを取得
		this.myRigidbody=GetComponent<Rigidbody>();
		//stateTextオブジェクトを取得
		this.stateText=GameObject.Find("GameResultText");
		//scoreTextオブジェクトを取得
		this.scoreText=GameObject.Find("ScoreText");

	}
	
	// Update is called once per frame
	void Update () {
		//ゲーム終了なら動きを止める
		if (this.isEnd) {
			this.forwardForce *= this.coefficient;
			this.turnForce *= this.coefficient;
			this.upForce *= this.coefficient;
			this.myAnimator.speed *= this.coefficient;
		}

		//前方向の力を加える
		this.myRigidbody.AddForce(this.transform.forward*this.forwardForce);
		//unityちゃんを入力に応じて左右に移動させる
		if ((Input.GetKey(KeyCode.LeftArrow)||this.isLButtonDown) && -this.movableRange < this.transform.position.x) {
			//左に移動
			this.myRigidbody.AddForce (-this.turnForce, 0, 0);
		} else if ((Input.GetKey (KeyCode.RightArrow) ||this.isRButtonDown)&& this.transform.position.x < this.movableRange) {
			//右に移動
			this.myRigidbody.AddForce (this.turnForce, 0, 0);
		}
		//ジャンプ中はジャンプを無効にする
		if(this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump")){
			this.myAnimator.SetBool("Jump",false);
		}
			//ジャンプしてない時にスペースが押されたらジャンプする
			if(Input.GetKeyDown(KeyCode.Space)&&this.transform.position.y<0.5f){
				//ジャンプアニメを再生
				this.myAnimator.SetBool("Jump",true);
				//上方向に力を加える
				this.myRigidbody.AddForce(this.transform.up*this.upForce);
			}
	}
	//他のオブジェクトと衝突した時の処理
	void OnTriggerEnter(Collider other){
		//障害物に衝突した場合
		if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag") {
			this.isEnd = true;
			this.stateText.GetComponent<Text> ().text = "GAME OVER";
		}
		//ゴール地点に到達した場合
		if (other.gameObject.tag == "GoalTag") {
			this.isEnd = true;
			this.stateText.GetComponent<Text> ().text = "CLEAR!!";
		}
		//コインに衝突した場合
		if (other.gameObject.tag == "CoinTag") {
			//スコアを加算
			this.score+=10;
			//獲得した点数を表示
			this.scoreText.GetComponent<Text>().text="Score  "+this.score+"pt";
			//パーティクルを再生
			GetComponent<ParticleSystem>().Play();

			//コインオブジェクトを破壊
			Destroy (other.gameObject);
		}
	}
	//ジャンプボタンを押した場合の処理
	public void GetMyJumpButtondown(){
		if (this.transform.position.y < 0.5f) {
			this.myAnimator.SetBool ("Jump", true);
			this.myRigidbody.AddForce (this.transform.up * this.upForce);
		}
	}
	//左ボタンを押した場合の処理
	public void GetMyLeftButtonDown(){
		this.isLButtonDown = true;
	}
	//離した時の処理
	public void GetMyLeftButtonUp(){
		this.isLButtonDown = false;
	}
	//右ボタン
	public void GetMyRightButtonDown(){
		this.isRButtonDown = true;
	}
	public void GetMyRightButtonUp(){
		this.isRButtonDown = false;
	}
}
