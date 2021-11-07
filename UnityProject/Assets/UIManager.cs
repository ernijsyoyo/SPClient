using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SP {
public class UIManager : Singleton<NetworkManagerTCP> {
    /// <summary>
    /// Reference to the Main Menu view
    /// </summary>
    [SerializeField]
    private GameObject _mMainPanel;

    #region NetworkMenu
    /// <summary>
    /// Reference to the Network Menu view
    /// </summary>
    [SerializeField]
    private GameObject _mNetworkMenu;
    /// <summary>
    /// UI element that denotes current status of TCP connection
    /// </summary>
    private Text _mTCPStatus;
    /// <summary>
    /// UI element that denotes the current status of OSC connection
    /// </summary>
    private Text _mOscStatus;
    /// <summary>
    /// UI element that denotes to which endpoint a socket will be opened
    /// </summary>
    private Text _mEndPoint;
    /// <summary>
    /// UI elemement that displays currently set values from the numberpad
    /// </summary>
    private Text _mKeyboardOutput;

    /// <summary>
    /// Displays all navigation destinations as child object toggle buttons
    /// </summary>
    [SerializeField]
    private GameObject navigationItems;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private GameObject navigationToggleButton;

    public List<GameObject> views = new List<GameObject>();
    public List<Toggle> destToggles = new List<Toggle>();
    #endregion

    private void Start() {
        OscReceiveHandler.OnDestinationsReceived += OscReceiveHandler_OnDestinationsReceived;    
    }
    private void OnDestroy() {
        OscReceiveHandler.OnDestinationsReceived -= OscReceiveHandler_OnDestinationsReceived;
    }


    private void OscReceiveHandler_OnDestinationsReceived(OscReceiveHandler.oscArgs args) {
        // Clear children to prevent double objects
        foreach (Transform child in navigationItems.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Set initial variables
        var destinations = args.msg.Values;
        var startingX = -325f;
        var startingY = 200f;
        var stepX = 325f;
        var stepY = 130f;
        var row = 1;
        var column = 1;

        // Loop over the amount of destination that we have and instantiate a gameobject
        // at default location. Increment the column and X position, once we reach
        // maximum column, start a new line by decreasing Y and return carriage to 1
        foreach (var item in destinations)
        {
            var dest = item.StringValue;
            var pos = new Vector3(startingX, startingY, 0);
            var go = Instantiate(navigationToggleButton, navigationItems.transform);
            go.transform.localPosition = pos;
            go.GetComponentInChildren<Text>().text = item.StringValue;
            go.gameObject.name = item.StringValue;
            destToggles.Add(go.GetComponent<Toggle>()); // used upon issuing navigation

            if(column != 3)
            {
                column++;
                startingX += stepX;
            }
            else {
                column = 1;
                startingX = -325f;
                startingY -= stepY;
                row++;
            }
        }
    }

    public void issueNavigation() {
        var outputDestinations = new List<string>();
        foreach (var dest in destToggles) {
            if (dest.isOn) {
                outputDestinations.Add(dest.gameObject.name);
            }
        }
        OSCSender.sendList(Constants.OSC_SET_DEST, outputDestinations);
    }

    

    public void refreshConnectionStatus() {
        _mTCPStatus.text = NetworkManagerTCP.Instance.getConnectionStatus();
        _mOscStatus.text = "Status check not implemented";
    }

    public string getKeyboardOutput() { return _mKeyboardOutput.text; }

    public void setKeyboardOutput(string value) {
        var textLength = _mKeyboardOutput.text.Length;
        if (value == "backspace") {
            _mKeyboardOutput.text = _mKeyboardOutput.text.Remove(textLength - 1);
        }
        else {
            _mKeyboardOutput.text += value;
        }
    }

    public string getEndpPointText() { return _mEndPoint.text; }

    public void setEndPoint() {
        var endPointValue = _mEndPoint.text;
        // Prevent an attempt to set the port before and IP address
        if (string.IsNullOrEmpty(endPointValue) && getKeyboardOutput().Length <= 4)
            return;
        // set IP
        else if (getKeyboardOutput().Length > 4)
            endPointValue += getKeyboardOutput() + ":";
        // set port
        else
            endPointValue += getKeyboardOutput();
    }

    /// <summary>
    /// Issue a connection to the TCP server
    /// </summary>
    public void issueTcpConnection() {
        var endPoint = getEndPoint();
        NetworkManagerTCP.Instance.ConnectToTcpServer(endPoint.Item1, endPoint.Item2);
    }

    public void disconnectTcp() {
        NetworkManagerTCP.Instance.disconnectTcp();
    }

    public Tuple<string, int> getEndPoint() {
        var endPoint = _mEndPoint.text.Split(':');
        var ip = endPoint[0];
        int port = 0;
        int.TryParse(endPoint[1], out port);
        return new Tuple<string, int>(ip, port);
    }

    public void changeView(string viewName)
    {
        foreach (var item in views) {
            if (item.name != viewName)
                item.SetActive(false);
            else
                item.SetActive(true);
        }
    }
}
} // end of namespace