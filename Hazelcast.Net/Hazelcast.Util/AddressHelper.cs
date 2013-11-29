using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Hazelcast.IO;
using Hazelcast.Net.Ext;
using Hazelcast.Util;


namespace Hazelcast.Util
{
	
	public sealed class AddressHelper
	{
		private const int MaxPortTries = 3;

		public static ICollection<IPEndPoint> GetSocketAddresses(string address)
		{
            AddressHolder addressHolder = AddressUtil.GetAddressHolder(address, -1);
			string scopedAddress = addressHolder.scopeId != null ? addressHolder.address + "%" + addressHolder.scopeId : addressHolder.address;
			IPAddress inetAddress = null;
			try
			{
				inetAddress = Address.GetAddressByName(scopedAddress);
			}
			catch (Exception e)
			{
			}
			return GetPossibleSocketAddresses(inetAddress, addressHolder.port, scopedAddress);
		}

	    public static ICollection<IPEndPoint> GetPossibleSocketAddresses(IPAddress ipAddress, int port, string scopedAddress)
		{
			int portTryCount = 1;
			if (port == -1)
			{
				portTryCount = MaxPortTries;
				port = 5701;
			}
			ICollection<IPEndPoint> socketAddresses = new List<IPEndPoint>();
			if (ipAddress == null)
			{
				for (int i = 0; i < portTryCount; i++)
				{
				    IPAddress _ipAddress;
                    IPAddress.TryParse(scopedAddress, out _ipAddress);

                    socketAddresses.Add(new IPEndPoint(_ipAddress, port + i));
				}
			}
			else
			{
				if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					for (int i = 0; i < portTryCount; i++)
					{
						socketAddresses.Add(new IPEndPoint(ipAddress, port + i));
					}
				}
				else
				{
                    //TODO IPv6 support should be checked.
                    //throw new NotSupportedException("IPv6 not supported yet");
                    var addresses = AddressUtil.GetPossibleInetAddressesFor(ipAddress);
                    foreach (var ipa in addresses)
                    {
                        for (int i = 0; i < portTryCount; i++)
                        {
                            socketAddresses.Add(new IPEndPoint(ipa, port + i));
                        }
                    }
				}
			}
			return socketAddresses;
		}


	    private AddressHelper()
		{
		}
	}
}