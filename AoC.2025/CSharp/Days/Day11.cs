using System;
using System.Runtime.InteropServices;
using AoC.Shared;

namespace AoC._2025.Days
{
    public class Day11 : IAoCDay
    {
        public string Part1(string input)
        {
            var lines = input.Split('\n');

            var devices = new Dictionary<string, Device>();

            foreach (var line in lines)
            {
                var devicesString = line.Split(' ');
                var thisDeviceString = devicesString[0][..^1];
                var outputDeviceStrings = devicesString[1..];

                if (devices.ContainsKey(thisDeviceString))
                {
                    devices[thisDeviceString].OutputDevices.AddRange(outputDeviceStrings.Select(x => devices.ContainsKey(x) ? devices[x] : new Device(x, new List<Device>())).ToList());
                }
                else
                {
                    var thisDevice = new Device(thisDeviceString, outputDeviceStrings.Select(x => devices.ContainsKey(x) ? devices[x] : new Device(x, new List<Device>())).ToList());
                    devices[thisDeviceString] = thisDevice;
                }
            }
            var countOfPathsThroughYou = 0;
            var visited = new HashSet<Device>();

            CountPaths(devices["svr"], ref countOfPathsThroughYou, devices, visited);

            return countOfPathsThroughYou.ToString();
        }

        public bool CountPaths(Device device,ref int paths, Dictionary<string, Device> devices, HashSet<Device> visited)
        {

            if (device.Name == "out")
                return true;

            foreach (var outputDevice in device.OutputDevices)
            {
                var thisDevice = devices.ContainsKey(outputDevice.Name) ? devices[outputDevice.Name] : outputDevice;
                if (CountPaths(thisDevice, ref paths, devices, visited))
                {
                    if (!visited.Contains(thisDevice)) visited.Add(thisDevice);
                    return true;
                }
            }

            return false;
        }


        public record Device
        {
            public List<Device> OutputDevices = new List<Device>();
            public string Name { get; set; }

            public Device(string name, List<Device> output)
            {
                Name = name;
                OutputDevices = output;
            }
        }

        public string Part2(string input)
        {
            throw new NotImplementedException();
        }
    }
}