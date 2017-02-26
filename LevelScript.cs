using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // generic list
using System.Linq;


public class LevelScript : MonoBehaviour {
	public GameObject[,] tablica = new GameObject[5,5];
	//public string[,] tablist = new string[,];
	public List <EnemyScript> listek = new List<EnemyScript> ();

	public int listint;
	public List <string> listalist1 = new List<string> ();
	public List <string> listalist = new List<string> ();
	public List< List<string> > listalist3 = new List< List<string> >();
	public List< List<string> > listaszer = new List< List<string> >();
//	public List<string> listaUtyl = new List<string>();
	public Vector3 pozycja;
	public GameObject Enemy;
	public GameObject Cube;
	public GameObject Scene;
	bool ruch = true;
	public float czasprzerwy =(1.5f);
	bool wlewo = true;
	public bool wdol = false;
//	public GUIText scoreText;
	public GameObject scoreText;
	public GameObject LivesText;
	public GameObject GOText;
	public static int score;
	GameObject obcy;
	string pomoc;
	bool alienReloaded=true;
	public static int wysSzer=2;
	public static int szerSzer=3;
	GameObject alien;
	public int zycia=3;
	bool koniecgry = true;
	float timemulti = 0.001f;
	int przysint=0;

	Text szkorr;
	Text txtt;
	Text MainText;
	public string sss;
	//public Text aaa;
	public GameObject[,] tablica2d = new GameObject[wysSzer,szerSzer];

	// Use this for initialization
	void Start () {
		GOText.SetActive (false);				//ukryj napis
		score = 0;
		txtt = scoreText.GetComponent<Text>();
		szkorr = LivesText.GetComponent<Text> ();
		MainText = GOText.GetComponent<Text> ();

		UpdateScore ();
		StartCoroutine ("NowyPoziom");
	

	//	AlienShoot ();
	}



	void nowaFala()

	{

		int y=0;
		int x=0;
		//tablica2d[x,y].Clear();
		for (x=0; x<szerSzer; x++) {
			listalist3.Add (new List<string> ());
			//listaszer.Add (new List<string>());
		}
		for (y=0; y<wysSzer; y++) {
			listaszer.Add (new List<string>());
			//listalist3.Add (new List<string> ());
		}
		
		for(x=0; x<szerSzer; x++)
		{
			
			
			for(y=0; y<wysSzer; y++)
			{
				
				pozycja=gameObject.transform.position + new Vector3(x*2,y*2,0);
				//	Debug.Log (pozycja);
				GameObject go = Instantiate (Enemy, pozycja,transform.rotation) as GameObject;
				go.gameObject.name = "Obcy x"+x+" y"+y;
				go.gameObject.tag = "Enemy";
				go.transform.parent = GameObject.Find("Scene").transform;
				
				listalist3[x].Add(go.name);
			}
			
		}
		PrzypSzer();
	
	}

	IEnumerator NowyPoziom()
	{
		listalist3.Clear ();
		listaszer.Clear ();
		koniecgry=true;
		GOText.SetActive (true);
		MainText.text="Get Ready";
		nowaFala ();
		yield return new WaitForSeconds(2.0f);
		GOText.SetActive (false);

		koniecgry=false;
		StopCoroutine ("NowyPoziom");
		
	}


	void PrzypSzer()			//przypisz szereg
	{

		int ax = 0;
		int ay = 0;

		for (ay=0; ay<listalist3.Count(); ay++) 
		{
			for (ax=0; ax<listalist3[ay].Count(); ax++) 
			{
				listaszer[ax].Add(listalist3[ay][ax]);
			}
		}

	}


	// Update is called once per frame
	void FixedUpdate () {
		if(koniecgry==false){
		if (ruch == true) {
			ruch = false;
			if(wdol){
				StartCoroutine ("RuchwDol");
			}
			else{
				StartCoroutine ("Ruch");
				}
			}
		

				}
	}

	void Przyspiesz()
	{	przysint++;
		if (przysint >= 10) {
				
						timemulti = timemulti + 0.1f;
						czasprzerwy = czasprzerwy - timemulti;
						przysint=0;	
				}
	}


	void GameOver()
	{

		if (zycia == 0) 
		{
			GOText.SetActive (true);
			MainText.text="Gejm Over";
			koniecgry=true;
		}
		else if (zycia > 0) {
						zycia--;
			StartCoroutine("ReSpawn");
				}

		UpdateScore ();
	//	GameObject go = Instantiate (Cube, pozycja,transform.rotation) as GameObject;
	
	}

	IEnumerator ReSpawn()
	{
		//	Debug.Log ("wachaj");
		pozycja=gameObject.transform.position + new Vector3(0,-10.0f,0);
		yield return new WaitForSeconds(2.0f);
		GameObject go = Instantiate (Cube, pozycja,transform.rotation) as GameObject;
		//reloaded = true;
		//	Debug.Log ("siura");
		
	}
	


	IEnumerator RuchwDol()
	{
		//Debug.Log ("ruchwdol");
		//alien.BroadcastMessage ("wDol", null, SendMessageOptions.DontRequireReceiver);

		for (int y=0; y<listaszer.Count(); y++) {
						for (int x=0; x<listaszer[y].Count(); x++) {
								GameObject.Find (listaszer [y] [x]).BroadcastMessage ("wDol", SendMessageOptions.DontRequireReceiver);
						}
			yield return new WaitForSeconds(czasprzerwy/10);
				}

	//	GameObject.Find ("Scene").BroadcastMessage ("wDol", SendMessageOptions.DontRequireReceiver);
		wdol = false;
		if (wlewo)
		{wlewo = false;
		}
		else
		{wlewo = true;}
		yield return new WaitForSeconds(czasprzerwy);
		ruch = true;
		StopCoroutine ("RuchwDol");
		//yield break;
		
		ruch = true;
		
	}

	IEnumerator Ruch()
	{

		//StartCoroutine ("ruchwBok");
		ruchwBok ();
		//AlienShoot ();	//strzela po ruchu w czasie (albo i przed) przerwa
		yield return new WaitForSeconds(czasprzerwy);
		ruch = true;
		StopCoroutine ("Ruch");

	}
	

	void ruchwBok()
	{

		if (wlewo) {
			for(int y=0;y<listaszer.Count();y++){
				for(int x=0;x<listaszer[y].Count();x++){
			
			//GameObject.Find ("Scene").BroadcastMessage ("wLewo", SendMessageOptions.DontRequireReceiver);
			GameObject.Find (listaszer[y][x]).BroadcastMessage ("wLewo", SendMessageOptions.RequireReceiver);
				//GameObject.Find ("Scene").BroadcastMessage ("wLewo", SendMessageOptions.DontRequireReceiver);
		//			Debug.Log ("poszedl"+listaszer[y][x]);

				}
				//yield return new WaitForSeconds(czasprzerwy/10);
		//		Debug.Log ("poszedl rzad"+y);
			}

		} 
		else if (wlewo==false)
		{
			for(int y=0;y<listaszer.Count();y++){
				for(int x=0;x<listaszer[y].Count();x++){
					
					//GameObject.Find ("Scene").BroadcastMessage ("wLewo", SendMessageOptions.DontRequireReceiver);
					GameObject.Find (listaszer[y][x]).BroadcastMessage ("wPrawo", SendMessageOptions.RequireReceiver);
					//GameObject.Find ("Scene").BroadcastMessage ("wLewo", SendMessageOptions.DontRequireReceiver);
				//	Debug.Log ("poszedl"+listaszer[y][x]);
					
				}
			//	yield return new WaitForSeconds(czasprzerwy/10);
			//	Debug.Log ("poszedl rzad"+y);
			}
			
		} 
	//	ruch = true;
		AlienShoot ();
		StopCoroutine ("ruchwBok");
	}

	void zmienkier()
	{
		wdol = true;
	}

	

	void UpdateScore()
	{
		txtt.text = "score: "+score;
		szkorr.text = "Lives: " +zycia;
	}

	public void AddScore()
	{
		score += 10;
		UpdateScore ();
	}

	void AlienShoot()
	{

		int szansaStrzalu = Random.Range (0, 10);
	//	Debug.Log ("Wylosowal");
		if ((szansaStrzalu < 2)&&(alienReloaded==true)) {
			alienReloaded = false;
		///	Debug.Log ("pierwszy if");
						int wylosowanyRzad = Random.Range (0, szerSzer); //wylosuj rzad
			if ((listalist3 [wylosowanyRzad].Count> 0)||(listalist3 [wylosowanyRzad]!=null)) //sprawdz czy lista nie jest pusta
			{
		//		Debug.Log ("drugi if");

							//	pomoc = listalist3 [wylosowanyRzad].First();
				obcy = GameObject.Find (listalist3 [wylosowanyRzad].First());
				//Debug.Log ("najnizszy element " + listalist3 [wylosowanyRzad].Last ());
								//GameObject obcy.name=Scene.Search(listalist3[wylosowanyRzad].First());
								//obcy = listalist3 [wylosowanyRzad].FirstOrDefault ();
				//EnemyScript uhu = obcy.GetComponent<EnemyScript>();

			//	obcy.BroadcastMessage ("AlienShoot", null, SendMessageOptions.RequireReceiver);  	//uzyealem tego przed pobieraniem componentu ze skryptu
								//listalist3[wylosowanyRzad].First().AlienShoot();
								//GameObject.Find ("Scene").BroadcastMessage ("AlienShoot",obcy, SendMessageOptions.DontRequireReceiver);
			//	StartCoroutine("alienReload");
		

			/*	EnemyScript uhu = obcy.GetComponent<EnemyScript>();
				uhu.AlienShoot();*/
				//listalist3 [wylosowanyRzad].RemoveAt (0);
						}
	//		Debug.Log ("wychodzi z ifa");
		/*	EnemyScript uhu = obcy.GetComponent<EnemyScript>();
			if(obcy!=null)
				uhu.AlienShoot();*/
				}
	//	Debug.Log ("Wyszedl z ifa2");

		//alienReloaded = true;
	}

	IEnumerator alienReload()
	{
		Debug.Log ("Wyczekane Reload");
		yield return new WaitForSeconds(3.0f);

		alienReloaded = true;
		StopCoroutine("alienReload");
	}


	public void usunzTab(GameObject alien)
	{

		int x = 0;
		int y = 0;

		for(int i=0;i<listaszer.Count();i++)
		{
			for(int j=0;j<listaszer[i].Count();j++)
				//	foreach(int j in  listalist3[i][y])
			{
				
				
				if(listaszer[i][j]==alien.name)
				{
					/*Debug.Log("element i"+i+" j"+j+" to:  "+listalist3[i][j]);
					
					Debug.Log("o kurwa! rowna sie");*/
					listaszer[i].RemoveAt(j);
			//		Debug.Log ("tablica: "+i+ "pozostalo "+listalist3[i].Count+"elementow");
					if (listaszer[i].Count()==0)
						listaszer.RemoveAt(i);
					Przyspiesz();
					//Debug.Log ("czasprzerwy: "+czasprzerwy+"multi"+"timemulti: "+timemulti);
					//			tablica[i,j].RemoveAt(j);
					
			/*		if (listalist3.Count==0)
						Debug.Log ("Wygrales");*/
		//			break;
				}
				
			}
		}

		for(int i=0;i<listalist3.Count();i++)
		{
			for(int j=0;j<listalist3[i].Count();j++)
			//	foreach(int j in  listalist3[i][y])
			{


				if(listalist3[i][j]==alien.name)
				{
				//	Debug.Log("element i"+i+" j"+j+" to:  "+listalist3[i][j]);
				
				//	Debug.Log("o kurwa! rowna sie");
					listalist3[i].RemoveAt(j);
					//Debug.Log ("tablica: "+i+ "pozostalo "+listalist3[i].Count+"elementow");
					if (listalist3[i].Count()==0)
						listalist3.RemoveAt(i);
					//			tablica[i,j].RemoveAt(j);

					if (listalist3.Count()==0){
						Debug.Log ("Wygrales");
						StartCoroutine("NowyPoziom");}
					break;
				}

			}
		}

	}
}
