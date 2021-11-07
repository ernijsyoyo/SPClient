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
    #endregion

    public void refreshConnectionStatus()  {
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

    public string getEndpPoint() { return _mEndPoint.text; }

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
}
} // end of namespace