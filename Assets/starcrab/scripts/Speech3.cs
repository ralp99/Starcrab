using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.Events;


public class Speech3 : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    [System.Serializable]

    public class instanceList

    {
        public string note = "";

        public string phrase = "";
        //  public GameObject[] usedObject;
        public UnityEvent ThisEvent;
    }

    public List<instanceList> InstanceList;



    //public GameObject[] hide, show;
    //public string hidePh, showPh = "";
  

    // Use this for initialization
    void Start()
    {


        {
            foreach (instanceList inList in InstanceList)
            {
                //   if (!inList.mute) Timeslist.Add(inList.duration);

                



               
                    keywords.Add(inList.phrase, () =>
                    {
                        // Call the OnReset method on every descendant object.
                        //  this.BroadcastMessage("OnReset");

                        /*

                                            foreach (GameObject picked in inList.usedObject)
                                                {
                                                    bool objectMode;
                                                   // if (!inList.hideObject) objectMode = true;
                                                    // picked.SetActive(objectMode);
                                                   picked.SetActive(!inList.hideObject);

                                                }
                                                */

                        inList.ThisEvent.Invoke();



                    });
               






            }

            //   highestTime = Mathf.Max(Timeslist.ToArray());
        }





        //keywords.Add(hidePh, () =>
        //{
        //    // Call the OnReset method on every descendant object.
        //    //  this.BroadcastMessage("OnReset");

        //    foreach (GameObject picked in hide)
        //    {
        //        picked.SetActive(false);
        //    }


        //});

        //keywords.Add(showPh, () =>
        //{
        //    // Call the OnReset method on every descendant object.
        //    //  this.BroadcastMessage("OnReset");

        //    foreach (GameObject picked in show)
        //    {
        //        picked.SetActive(true);
        //    }


        //});



        //keywords.Add("Reset world", () =>
        //{
        //    // Call the OnReset method on every descendant object.
        //    this.BroadcastMessage("OnReset");
        //});

        //keywords.Add("Drop Sphere", () =>
        //{
        //    var focusObject = GazeGestureManager.Instance.FocusedObject;
        //    if (focusObject != null)
        //    {
        //        // Call the OnDrop method on just the focused object.
        //        focusObject.SendMessage("OnDrop");
        //    }
        //});

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
