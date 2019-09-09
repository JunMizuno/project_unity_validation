using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using UnityEngine.Profiling;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 取得数値などのテスト検証用のクラス
/// </summary>
public class TestData : MonoBehaviour
{
    private int[] sampleIntArray = new int[1];
    private List<int> sampleIntList = new List<int>();
    private Dictionary<int, int> sampleIntDictionary = new Dictionary<int, int>();
    private Queue<int> sampleIntQueue = new Queue<int>();
    private Stack<int> sampleIntStack = new Stack<int>();

    // @memo. デリゲートテスト
    delegate void ShowLog(string str);
    delegate int CalculateBase(int a, int b);

    // @memo.
    private Vector3[] vectorArray;

    // @memo. 非同期テスト
    private CancellationTokenSource tokenSource;

    private void Awake()
    {

    }

    private void Start()
    {
        //TestLerp();
        //TestSLerp();
        //TestDelegate();
        //TestLINQ();
        //TestLoopPatern();

        //StartCoroutine("TestCoroutine");
        //StopCoroutine("TestCoroutine");
        //StopAllCoroutines();

        //StartCoroutine("TestDebugMessage");

        //ExecuteAsync();

        /*
        List<int> list1 = new List<int>() { 0, 1, 2, 3, 4, 3, 2, 1, 0 };
        List<int> list2 = new List<int>() { 7, 8, 9, 8, 7 };
        List<int> list3 = new List<int>() { 4, 5, 6, 5, 4 };

        List<int> newList1 = new List<int>() { 0, 1, 2, 3, 4, 5 };
        newList1.RemoveAtValue(c => c == 3);

        string log = "";
        foreach (var data in newList1)
        {
            log += " " + data;
        }
        Debug.Log(log.WithColorTag(Color.cyan));
        */
    }

    private void Update()
    {
        //GetAxisData();

        //Profiler.BeginSample("TestProfiler");
        //TestProfiler();
        //Profiler.EndSample();
    }

    private void OnEnable()
    {
        if (tokenSource == null)
        {
            tokenSource = new CancellationTokenSource();
        }
    }

    private void OnDisable()
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
        }
    }

    private void OnDestroy()
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
        }
    }

    private void OnGUI()
    {

    }

    private void GetAxisData()
    {
        if (Input.GetMouseButton(0))
        {
            // @memo. スクロールやスライドすると値が取れる、それ以外はゼロが返る
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            Debug.LogFormat("<color=white>GetAxis x:{0} y:{1}</color>", x, y);
        }
    }

    private void TestArray()
    {
        // @memo. 第1引数に値、第2引数にインデックス
        sampleIntArray.SetValue(5, 0);
        Debug.Log("<color=white>" + "sampleIntArrayの要素数:" + sampleIntArray.Length + "</color>");
        Debug.LogFormat("<color=white>sampleIntArrayのインデックス:{0} その値:{1}</color>", 0, sampleIntArray.GetValue(0));
    }

    private void TestList()
    {
        sampleIntList.Add(1);
        sampleIntList.Add(2);
        sampleIntList.Add(3);

        Debug.Log("<color=white>" + "sampleIntListの要素数:" + sampleIntList.Count + "</color>");

        foreach (int value in sampleIntList)
        {
            Debug.Log("<color=white>" + "sampleIntList value:" + value + "</color>");
        }

        sampleIntList.Clear();
        Debug.Log("<color=white>" + "sampleIntListのClear後の要素数:" + sampleIntList.Count + "</color>");
    }

    private void TestDictionary()
    {
        // @memo. キーの上書きは出来ない
        sampleIntDictionary.Add(0, 1);
        sampleIntDictionary.Add(1, 2);
        sampleIntDictionary.Add(2, 4);

        Debug.Log("<color=cyan>" + "sampleIntDictionaryの要素数:" + sampleIntDictionary.Count + "</color>");

        foreach (var data in sampleIntDictionary)
        {
            Debug.LogFormat("<color=cyan>sampleIntDictionary Key:{0} Value:{1}</color>", data.Key, data.Value);
        }

        sampleIntDictionary.Remove(1);
        Debug.Log("<color=cyan>" + "sampleIntDictionaryのRemove後の要素数:" + sampleIntDictionary.Count + "</color>");
    }

    private void TestQueue()
    {
        sampleIntQueue.Enqueue(5);
        sampleIntQueue.Enqueue(4);
        sampleIntQueue.Enqueue(3);
        sampleIntQueue.Enqueue(2);
        sampleIntQueue.Enqueue(1);

        Debug.Log("<color=white>" + "sampleIntQueueの要素数:" + sampleIntQueue.Count + "</color>");

        int value = sampleIntQueue.Dequeue();

        Debug.Log("<color=white>" + "sampleIntQueueのDequeue後の要素数:" + sampleIntQueue.Count + " Dequeueの値:" + value + "</color>");

    }

    private void TestStack()
    {
        sampleIntStack.Push(9);
        sampleIntStack.Push(8);
        sampleIntStack.Push(7);
        sampleIntStack.Push(6);
        sampleIntStack.Push(5);

        Debug.Log("<color=white>" + "sampleIntStackの要素数:" + sampleIntStack.Count + "</color>");

        int value = sampleIntStack.Pop();

        Debug.Log("<color=white>" + "sampleIntStackのPop後の要素数:" + sampleIntStack.Count + " Popの値:" + value + "</color>");
    }

    private void TestLerp()
    {
        float floatValue1 = Mathf.Lerp(0.0f, 10.0f, 0.5f);
        Debug.Log("<color=cyan>" + "Leap(floatValue1)の中間点:" + floatValue1 + "</color>");

        float floatValue2 = Mathf.Lerp(10.0f, 20.0f, 0.5f);
        Debug.Log("<color=cyan>" + "Leap(floatValue2)の中間点:" + floatValue2 + "</color>");

        float floatValue3 = Mathf.Lerp(floatValue1, floatValue2, 0.5f);
        Debug.Log("<color=cyan>" + "Leap(floatValue3)の中間点:" + floatValue3 + "</color>");

        Vector3 vectorValue1 = Vector3.Lerp(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(10.0f, 10.0f, 10.0f), 0.5f);
        Debug.Log("<color=cyan>" + "Leap(VectorValue1)の中間点:" + vectorValue1 + "</color>");

        Vector3 vectorValue2 = Vector3.Lerp(new Vector3(5.0f, 5.0f, 5.0f), new Vector3(15.0f, 15.0f, 15.0f), 0.5f);
        Debug.Log("<color=cyan>" + "Leap(VectorValue2)の中間点:" + vectorValue2 + "</color>");

        Vector3 vectorValue3 = Vector3.Lerp(vectorValue1, vectorValue2, 0.5f);
        Debug.Log("<color=cyan>" + "Leap(VectorValue3)の中間点:" + vectorValue3 + "</color>");
    }

    private void TestSLerp()
    {
        int objectCount = 8;

        Vector3 topPoint1 = new Vector3(-5.0f, 0.0f, 0.0f);
        Vector3 middlePoint1 = new Vector3(0.0f, 0.0f, 5.0f);
        Vector3 bottomPoint1 = new Vector3(5.0f, 0.0f, 0.0f);

        for (float t = 0.0f; t <= 1.0f; t += (1.0f / (float)objectCount))
        {
            Vector3 firstSleapPoint = Vector3.Slerp(topPoint1, middlePoint1, t);
            Vector3 secondSlerpPoint = Vector3.Slerp(middlePoint1, bottomPoint1, t);
            Vector3 finallySlerpPoint = Vector3.Slerp(firstSleapPoint, secondSlerpPoint, t);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = finallySlerpPoint;
            Material material = new Material(Shader.Find("Standard")) { color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f) };
            UtilityRenderer.SetBlendMode(material, UtilityRenderer.Mode.Transparent);
            sphere.GetComponent<Renderer>().material = material;
        }

        Vector3 topPoint2 = new Vector3(-5.0f, 0.0f, 0.0f);
        Vector3 middlePoint2 = new Vector3(0.0f, 0.0f, 5.0f);
        Vector3 bottomPoint2 = new Vector3(5.0f, 0.0f, 0.0f);

        for (float t = 0.0f; t <= 1.0f; t += (1.0f / (float)objectCount))
        {
            Vector3 firstLeapPoint = Vector3.Lerp(topPoint2, middlePoint2, t);
            Vector3 secondLerpPoint = Vector3.Lerp(middlePoint2, bottomPoint2, t);
            Vector3 finallyLerpPoint = Vector3.Lerp(firstLeapPoint, secondLerpPoint, t);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sphere.transform.position = finallyLerpPoint;
            Material material = new Material(Shader.Find("Standard")) { color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.5f) };
            UtilityRenderer.SetBlendMode(material, UtilityRenderer.Mode.Transparent);
            sphere.GetComponent<Renderer>().material = material;
        }
    }

    private void TestDelegate()
    {
        ShowLog testShowLog1 = new ShowLog(str => Debug.Log(str));
        testShowLog1("デリゲートのnewでの生成テスト");

        ShowLog testShowLog2 = delegate (string str)
        {
            Debug.Log("デリゲートの匿名メソッドでの生成テスト:" + str);
        };
        testShowLog2("1回目");

        testShowLog2 = delegate (string str)
        {
            Debug.Log("デリゲートの匿名メソッドを代入する形:" + str);
        };
        testShowLog2("2回目");

        // @memo. 複数引数の場合はラムダでないと表現できないかも
        CalculateBase testPlus1 = new CalculateBase((int a, int b) => a + b);
        Debug.Log("new生成で 1 + 1 = " + testPlus1(1, 1));

        testPlus1 = delegate (int a, int b)
        {
            return (a + b) * 2;
        };
        Debug.Log("匿名メソッドで 1 + 1 = " + testPlus1(1, 1));

        // @memo. それぞれの関数の処理を順番に行う
        // @memo. testAddMethod2のような場合は最後の計算値のみが返ってくる(当然総合値ではない)ので、使い方としては不適当
        ShowLog testAddMethod1 = new ShowLog(x => Debug.Log("new ShowLog:" + x));
        testAddMethod1 += ShowDebugLog;
        testAddMethod1("表示テスト");

        CalculateBase testAddMethod2 = new CalculateBase((int a, int b) => a);
        testAddMethod2 += CalcAddition;
        testAddMethod2 += CalcSubtraction;
        Debug.Log("複数メソッドを格納して 10 と 20 を渡した場合:" + testAddMethod2(10, 20));
    }

    private void ShowDebugLog(string str)
    {
        Debug.Log("ShowDebugLog:" + str);
    }

    private int CalcAddition(int a, int b)
    {
        return a + b;
    }

    private int CalcSubtraction(int a, int b)
    {
        return a - b;
    }

    private void TestLINQ()
    {
        vectorArray = new Vector3[6];

        vectorArray[0] = new Vector3(1.0f, 0.0f, 1.0f);
        vectorArray[1] = new Vector3(2.0f, 0.0f, 0.0f);
        vectorArray[2] = new Vector3(3.0f, 3.0f, 0.0f);
        vectorArray[3] = new Vector3(4.0f, 0.0f, -1.0f);
        vectorArray[4] = new Vector3(5.0f, 0.0f, 0.0f);
        vectorArray[5] = new Vector3(6.0f, 3.0f, 0.0f);

        // @memo. 抽出するだけ
        var query1 = vectorArray
            .Where(vec => vec.x > 3.0f);
        Debug.Log("query1 " + string.Join(", ", query1));

        // @memo. 値はコピーされて使用される 
        var query2 = vectorArray
            .Select(vec => vec.x = 0.0f);
        Debug.Log("query2 " + string.Join(", ", query2));

        // @memo. 元の値は変化していない
        Debug.Log("vectorArray " + string.Join(", ", vectorArray));

        // @memo. MoveNextで回す
        IEnumerator e = vectorArray.GetEnumerator();
        while (e.MoveNext())
        {
            var data = (Vector3)e.Current;
            Debug.Log("vectorArray:" + data);
        }
    }

    private void TestProfiler()
    {
        List<int> list1 = new List<int>(2000000);
        for (int i = 0; i < 2000000; i++)
        {
            list1.Add(i);
        }

        List<int> list2 = new List<int>(1000000);
        for (int i = 0; i < 1000000; i++)
        {
            list2.Add(i);
        }
    }

    private void TestLoopPatern()
    {
        // @memo. ループの回り方をチェック
        foreach (var value in GetNumber())
        {
            Debug.Log("<color=cyan>" + "TestLoopPatern() foreachで取得した数値:" + value + "</color>");
            if (Console.KeyAvailable)
            {
                Debug.Log("<color=white>" + "キーが押されたので終了" + "</color>");
                return;
            }
        }

        var e = GetNumber().GetEnumerator();
        while (e.MoveNext())
        {
            Debug.Log("<color=green>" + "TestLoopPatern() whileで取得した数値:" + e.Current + "</color>");
        }

    }

    private IEnumerable<int> GetNumber()
    {
        // @memo. 検証用に複数回回す
        for (int i = 0; i < 100; i++)
        {
            foreach (var value in Enumerable.Range(1, 3))
            {
                Debug.Log("<color=magenta>" + "GetNumber() 取得した数値:" + value + "</color>");
                yield return value;
            }
        }
    }

    private IEnumerator TestCoroutine()
    {
        // @memo. この場合はフレームごとに1つの処理を行う
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("<color=white>" + "yield return null のループ:" + i + "</color>");
            yield return null;
        }

        yield return new WaitForSeconds(5.0f);
        Debug.Log("<color=red>" + "ウエイト終了後" + "</color>");

        // @memo. この場合は処理中断、次フレーム以降継続はしない
        /*
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("<color=white>" + "yield break のループ:" + i + "</color>");
            yield break;
        }
        */

        yield return true;
    }

    private IEnumerator<int> TestCoroutineWithReturnValue()
    {
        yield return 1;
    }

    private IEnumerator<bool> TestCoroutineWithRetuanBool()
    {
        yield return true;
    }

    private async Task TestAsyncFunc(CancellationToken cancelToken)
    {
        for (int i = 0; i < 10; i++)
        {
            await Task.Run(() =>
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }

                // 非同期したい処理をここに

            }).ContinueWith((obj) =>
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }

                Debug.Log(string.Format("Task.Run() {0}番目の処理 スレッドナンバー:{1}", i + 1, Thread.CurrentThread.ManagedThreadId));
            });

            await Task.Delay(2000).ContinueWith((obj) =>
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }

                // 非同期死体処理をここに

                Debug.Log(string.Format("Task.Delay() {0}番目の処理 スレッドナンバー:{1}", i + 1, Thread.CurrentThread.ManagedThreadId));
            });
        }
    }

    private void ExecuteAsync()
    {
        if (tokenSource != null)
        {
            var cancelToken = tokenSource.Token;
            Task.Run(() => TestAsyncFunc(cancelToken));
        }
    }
}
