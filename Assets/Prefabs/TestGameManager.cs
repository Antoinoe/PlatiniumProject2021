using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Management;
using System.Net.NetworkInformation;
using System;



public enum EPlayerNum
{
    Player1,
    Player2,
    Player3,
    Player4
}
//global::InTheHand.Net.Sockets.BluetoothDeviceInfo
public class TestGameManager : MonoBehaviour
{
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayers();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
            //SetupDevices();
    }

    void CreatePlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject player = new GameObject();
            player.name = "Player" + (i + 1).ToString();
            player.transform.position = new Vector2(0, -i);
            SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            player.AddComponent<Controller>();
            switch (i)
            {
                case 0:
                    sr.color = Color.white;
                    player.GetComponent<Controller>().SetPlayerNum(EPlayerNum.Player1);
                    break;

                case 1:
                    sr.color = Color.green;
                    player.GetComponent<Controller>().SetPlayerNum(EPlayerNum.Player2);
                    break;

                case 2:
                    sr.color = Color.red;
                    player.GetComponent<Controller>().SetPlayerNum(EPlayerNum.Player3);
                    break;

                case 3:
                    sr.color = Color.blue;
                    player.GetComponent<Controller>().SetPlayerNum(EPlayerNum.Player4);
                    break;
            } 
        }
    }

    /*List<BluetoothDeviceInfo> deviceList;

    void SetupDevices()
    {

        // mac is mac address of local bluetooth device
        BluetoothEndPoint localEndpoint = new BluetoothEndPoint(BluetoothAddress.Parse(GetMacAddress()), BluetoothService.SerialPort);
        Debug.Log("1");
        // client is used to manage connections
        BluetoothClient localClient = new BluetoothClient(localEndpoint);
        Debug.Log("2");
        // component is used to manage device discovery
        BluetoothComponent localComponent = new BluetoothComponent(localClient);
        // async methods, can be done synchronously too
        localComponent.DiscoverDevicesAsync(255, true, true, true, true, null);
        localComponent.DiscoverDevicesProgress += new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesProgress);
        localComponent.DiscoverDevicesComplete += new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesComplete);
        Debug.Log("DoneSetupDevices");
    }

    private void component_DiscoverDevicesProgress(object sender, DiscoverDevicesEventArgs e)
    {
        // log and save all found devices
        for (int i = 0; i < e.Devices.Length; i++)
        {
            if (e.Devices[i].Remembered)
            {
                Debug.Log(e.Devices[i].DeviceName + " (" + e.Devices[i].DeviceAddress + "): Device is known");
            }
            else
            {
                Debug.Log(e.Devices[i].DeviceName + " (" + e.Devices[i].DeviceAddress + "): Device is unknown");
            }
            this.deviceList.Add(e.Devices[i]);
        }
    }

    private void component_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
    {
        // log some stuff
    }



    /// <summary>
    /// Finds the MAC address of the NIC with maximum speed.
    /// </summary>
    /// <returns>The MAC address.</returns>
    private string GetMacAddress()
    {
        const int MIN_MAC_ADDR_LENGTH = 12;
        string macAddress = string.Empty;
        long maxSpeed = -1;

        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            Debug.Log(
                "Found MAC Address: " + nic.GetPhysicalAddress() +
                " Type: " + nic.NetworkInterfaceType);

            string tempMac = nic.GetPhysicalAddress().ToString();
            if (nic.Speed > maxSpeed &&
                !string.IsNullOrEmpty(tempMac) &&
                tempMac.Length >= MIN_MAC_ADDR_LENGTH)
            {
                Debug.Log("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                maxSpeed = nic.Speed;
                macAddress = tempMac;
            }
        }

        return macAddress;
    }*/
}
