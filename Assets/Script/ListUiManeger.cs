using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListUiManeger : MonoBehaviour
{
    public TextMeshProUGUI StatusText;
    public GameObject SessionList;
    public VerticalLayoutGroup ListLineContent;

    public void ClearList()
    {
        foreach (Transform child in ListLineContent.transform)
        {
            Destroy(child.gameObject);
        }
        StatusText.gameObject.SetActive(false);
    }

    public void AddList(SessionInfo sessionInfo)
    {
        ListUILine addSessionInforUiList = Instantiate(SessionList, ListLineContent.transform).GetComponent<ListUILine>();
        addSessionInforUiList.SetInformation(sessionInfo);
        addSessionInforUiList.OnJoinSession += AddSessionInforUiList;
    }

    private void AddSessionInforUiList(SessionInfo obj )
    {

    }

    public void OnNoSessionFound()
    {
        StatusText.text = "No Game Room Found";
        StatusText.gameObject.SetActive(true);
    }
    
    public void onLookingForRoom()
    {
        StatusText.text = "Looking For Game Room";
        StatusText.gameObject.SetActive(true);
    }
}
