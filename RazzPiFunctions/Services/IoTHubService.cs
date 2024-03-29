﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace RazzPiFunctions.Services
{
    public static class IoTHubService
    {
            const string connectionString = "CONNECTION STRING to your device";
            const string deviceID = "DEVICE-ID";

            const string primaryThumbprint = "<replace_with_certificate_thumbprint>";
            const string secondaryThumbprint = "<replace_with_certificate_thumbprint>";

            static void CheckTwins()
            {
                var manager = RegistryManager.CreateFromConnectionString(connectionString);

                var query = manager.CreateQuery("select * from devices");
                var twins = new List<Microsoft.Azure.Devices.Shared.Twin>();
                while (query.HasMoreResults)
                {
                    try
                    {
                        twins = query.GetNextAsTwinAsync().Result.ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex}");
                    }

                }

            }

            public static async Task SendMessage()
            {
                var serviceClient = ServiceClient.CreateFromConnectionString(connectionString, TransportType.Amqp);
                var str = "Hello, Cloud!";
                var message = new Message(Encoding.ASCII.GetBytes(str));
                await serviceClient.SendAsync(deviceID, message);
                Console.WriteLine("C2D Message Sent");
            }

            static async Task AddDeviceAsync()
            {
                RegistryManager manager = RegistryManager.CreateFromConnectionString(connectionString);
                await manager.AddDeviceAsync(new Device(deviceID));
                Console.WriteLine("Device Added");
            }

            static async Task AddDeviceWithSelfSignedCertificateAsync()
            {
                RegistryManager manager = RegistryManager.CreateFromConnectionString(connectionString);
                var device = new Device(deviceID)
                {
                    Authentication = new AuthenticationMechanism
                    {
                        Type = AuthenticationType.SelfSigned,
                        X509Thumbprint = new X509Thumbprint
                        {
                            PrimaryThumbprint = primaryThumbprint,
                            SecondaryThumbprint = secondaryThumbprint
                        }
                    }
                };
                await manager.AddDeviceAsync(device);
                Console.WriteLine("Device Added");
            }

            static async Task AddDeviceWithCertificateAuthorityAuthenticationAsync()
            {
                RegistryManager manager = RegistryManager.CreateFromConnectionString(connectionString);
                var device = new Device(deviceID)
                {
                    Authentication = new AuthenticationMechanism
                    {
                        Type = AuthenticationType.CertificateAuthority
                    }
                };
                await manager.AddDeviceAsync(device);
                Console.WriteLine("Device Added");
            }

            static async Task RemoveDeviceAsync()
            {
                RegistryManager manager = RegistryManager.CreateFromConnectionString(connectionString);
                await manager.RemoveDeviceAsync(deviceID);
                Console.WriteLine("Device Removed");
            }
        }
    }

