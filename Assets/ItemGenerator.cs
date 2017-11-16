using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour {
	//それぞれのPrefabを入れる
	public GameObject carPrefab;
	public GameObject coinPrefab;
	public GameObject conePrefab;
	//スタート地点
	private int startPos=-160;
	//ゴール地点
	private int goalPos=30;
	//アイテムを生成するx軸の範囲
	private float posRange=3.4f;
	//unityちゃん
	private GameObject unitychan;
	//unityちゃんの位置
	private float uniPos;
	//アイテム生成時のunityちゃんの位置
	private float itemCleatedPos;
	//アイテムの生成位置
	private float itemStartPos;
	private float itemEndPos;

	private int num;

	//アイテム生成関数
	void Item(){
		//一定の位置でアイテム生成
		for (float i = itemStartPos; i < itemEndPos; i += 15) {
			//アイテムを生成した時のunityちゃんの位置を取得
			this.itemCleatedPos = this.unitychan.transform.position.z;
			//生成するアイテムをランダムに設定
			this.num = Random.Range (0, 10);
			if (num <= 1) {
				//コーンを横一直線に生成
				for (float j = -1; j <= 1; j += 0.4f) {
					GameObject cone = Instantiate (conePrefab)as GameObject;
					cone.transform.position = new Vector3 (4 * j, cone.transform.position.y, i);
				}
			} else {
				//レーンごとにアイテムを生成
				for (int j = -1; j < 2; j++) {
					//アイテムの種類を決める
					int item = Random.Range (1, 11);
					//アイテムを置くZ座標をランダムに設定
					int offsetZ = Random.Range (-5, 6);
					//60%コイン、30％車、10％何もなし
					if (1 <= item && item <= 6) {
						GameObject coin = Instantiate (coinPrefab)as GameObject;
						coin.transform.position = new Vector3 (posRange * j, coin.transform.position.y, i + offsetZ);
					} else if (7 <= item && item <= 9) {
						//車を生成
						GameObject car = Instantiate (carPrefab)as GameObject;
						car.transform.position = new Vector3 (posRange * j, car.transform.position.y, i + offsetZ);
					}
				}
			}
		}
	}
		

	// Use this for initialization
	void Start () {
		//オブジェクト取得
		this.unitychan = GameObject.Find ("unitychan");

	}

	// Update is called once per frame
	void Update () {
		//unityちゃんの位置を取得
		this.uniPos = unitychan.transform.position.z;
		//アイテムの生成位置
		this.itemStartPos = uniPos + 50;
		this.itemEndPos = itemStartPos + 50;
		//unityちゃんがある一定の位置に行くまでアイテム生成
		if (this.uniPos <= this.goalPos) {
			//最初のアイテム生成
			if (this.uniPos <= -245) {
				//関数呼び出し
				Item();
				//unityちゃんの進行に応じてアイテム生成
			} else if (uniPos - itemCleatedPos >= 60) {
				//関数呼び出し
				Item();
			}
		}
	}
}