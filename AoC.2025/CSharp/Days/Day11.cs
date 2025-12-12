using AoC.Shared;
using System.ComponentModel;

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

                var outputDevices = outputDeviceStrings.Select(x => devices.ContainsKey(x) ? devices[x] : new Device(x, new List<Device>()));

                // inefficient, but graph size likely doesn't matter here.
                foreach (var oDevice in outputDevices)
                {
                    devices.TryAdd(oDevice.Name, oDevice);
                }

                if (devices.ContainsKey(thisDeviceString))
                {
                    devices[thisDeviceString].OutputDevices.AddRange(outputDevices);
                }
                else
                {
                    var thisDevice = new Device(thisDeviceString, outputDevices.ToList());
                    devices[thisDeviceString] = thisDevice;
                }
            }
            var countOfPathsThroughYou = 0;
            var visited = new HashSet<Device>();
            Dictionary<string, int> memo = new();

            foreach (var device in devices["you"].OutputDevices)
            {
                countOfPathsThroughYou += CountPaths(device, devices, memo);
            }

            return countOfPathsThroughYou.ToString();
        }



        public string Part2(string input)
        {
            var lines = input.Split('\n');

            var devices = new Dictionary<string, Device>();

            foreach (var line in lines)
            {
                var devicesString = line.Split(' ');
                var thisDeviceString = devicesString[0][..^1];
                var outputDeviceStrings = devicesString[1..];

                var outputDevices = outputDeviceStrings.Select(x => devices.ContainsKey(x) ? devices[x] : new Device(x, new List<Device>()));

                // inefficient, but graph size likely doesn't matter here.
                foreach (var oDevice in outputDevices)
                {
                    devices.TryAdd(oDevice.Name, oDevice);
                }

                if (devices.ContainsKey(thisDeviceString))
                {
                    devices[thisDeviceString].OutputDevices.AddRange(outputDevices);
                }
                else
                {
                    var thisDevice = new Device(thisDeviceString, outputDevices.ToList());
                    devices[thisDeviceString] = thisDevice;
                }
            }
            var countOfPathsThroughYou = 0L;
            var visited = new HashSet<Device>();
            Dictionary<(string, bool, bool), long> memo = new();

            foreach (var device in devices["svr"].OutputDevices)
            {
                countOfPathsThroughYou += CountPathsWithFftDac(device, devices, memo, false, false);
            }

            return countOfPathsThroughYou.ToString();
        }

        public long CountPathsWithFftDac(Device device, Dictionary<string, Device> devices, Dictionary<(string, bool, bool), long> memo, bool fft, bool dac)
        {
            if (device.Name == "out" && fft && dac)
                return 1;

            if (device.Name == "fft") fft = true;
            if (device.Name == "dac") dac = true;

            var numReachedOut = 0l;

            if (memo.ContainsKey((device.Name, fft, dac)))
            {
                return memo[(device.Name, fft, dac)];
            }
            else
            {
                foreach (var outputDevice in device.OutputDevices)
                {
                    var thisDevice = devices.ContainsKey(outputDevice.Name) ? devices[outputDevice.Name] : outputDevice;
                    numReachedOut += CountPathsWithFftDac(thisDevice, devices, memo, fft, dac);
                }
            }

            memo[(device.Name, fft, dac)] = numReachedOut;
            return numReachedOut;
        }

        public int CountPaths(Device device, Dictionary<string, Device> devices, Dictionary<string, int> memo)
        {
            if (device.Name == "out")
                return 1;

            var numReachedOut = 0;
            
            if (memo.ContainsKey(device.Name))
            {
                return memo[device.Name];
            }
            else
            {
                foreach (var outputDevice in device.OutputDevices)
                {
                    var thisDevice = devices.ContainsKey(outputDevice.Name) ? devices[outputDevice.Name] : outputDevice;
                    numReachedOut += CountPaths(thisDevice, devices, memo);
                }
            }

            memo[device.Name] = numReachedOut;
            return numReachedOut;
        }

        public class Device
        {
            public List<Device> OutputDevices = new List<Device>();
            public string Name { get; set; }

            public Device(string name, List<Device> output)
            {
                Name = name;
                OutputDevices = output;
            }
        }
    }
}