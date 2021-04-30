using System;
using System.Collections.Generic;
using System.IO;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace InterpretingThePackets
{
    class Program
    {
        private static string fileName = "SV.pcap";
        private static int counter;
        private static string[][] arr = new string[18317][];
        static void Main(string[] args)
        {
            if( FileCheck(fileName) )
            {
                PacketDevice selectedDevice = new OfflinePacketDevice(fileName);
                // Open the device
                using ( PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000) )
                {
                    communicator.ReceivePackets(0, PacketHandler);
                }
            }

            Console.WriteLine(arr);
            Console.ReadLine();
        }

        // Callback function invoked by libpcap for every incoming packet
        private static void PacketHandler(Packet packet)
        {
            //DEBUG MODE
            //Console.WriteLine(packet);

            counter++;

            arr[counter - 1] = new string[4] {
                counter.ToString(),
                packet.Ethernet.Arp.HardwareType.ToString(),
                packet.Ethernet.Source.ToString(),
                packet.Ethernet.Destination.ToString()
            };

            //Console.WriteLine("ID: " + counter++);
            //Console.WriteLine("APP ID: {0}", packet.Ethernet.Arp.HardwareType.ToString()); // APP ID
            //Console.WriteLine("MAC SOURCE: {0}", packet.Ethernet.Source.ToString());
            //Console.WriteLine("MAC Destination: {0}", packet.Ethernet.Destination.ToString());
        }

        public static bool FileCheck(string file)
        {
            return File.Exists(file);
        }
    }
}
