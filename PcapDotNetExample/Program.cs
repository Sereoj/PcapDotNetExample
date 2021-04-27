using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace PcapDotNetExample
{
    class Program
    {
        private const string FileName = "SV.pcap";
        static void Main(string[] args)
        {
            if(FileExists(FileName))
            {
                //Подлючаемся к файлу 
                OfflinePacketDevice selectedDevice = new OfflinePacketDevice(FileName);
                // Открываем
                using ( PacketCommunicator communicator = selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000) )
                {
                    Packet packet; // наш пакет
                    do
                    {
                        PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);
                        switch ( result )
                        {
                            case PacketCommunicatorReceiveResult.Timeout: 
                                // превышен лимит
                            continue;
                            case PacketCommunicatorReceiveResult.Ok:
                            //чтение 
                            Console.WriteLine(packet.Ethernet.Arp.Operation);
                            Console.WriteLine(packet.Ethernet.Destination);
                            break;
                            default:
                                //ошибка
                            throw new Exception();
                        }
                    } while ( true );
                }
            }
            else
            {
                Console.WriteLine("Нет указаного файла {0}", FileName);
            }
            


            Console.ReadLine();

        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}
