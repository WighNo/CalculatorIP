using CalculatorIP.Model.Data.IP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorIP.Model.Data.Mask
{
    public static class ListNetmask
    {
        private static Dictionary<IPAdressType, Netmask> _defaultNetmasks = new Dictionary<IPAdressType, Netmask>();

        private static Dictionary<int, Netmask> _listNetmask = new Dictionary<int, Netmask>();
        private static Dictionary<IPAdressType, NetmaskIndexRange> _netmaskRange = new Dictionary<IPAdressType, NetmaskIndexRange>();

        private static Netmask[] _netmasks = new Netmask[23]
        {
            new Netmask(new byte[] { 255, 128, 0, 0 }, 2, 8388606, NetmaskClass.A128),
            new Netmask(new byte[] { 255, 192, 0, 0 }, 4, 4194302, NetmaskClass.A64),
            new Netmask(new byte[] { 255, 224, 0, 0 }, 8, 2097150, NetmaskClass.A32),
            new Netmask(new byte[] { 255, 240, 0, 0 }, 16, 1048574, NetmaskClass.A16),
            new Netmask(new byte[] { 255, 248, 0, 0 }, 32, 524286, NetmaskClass.A8),
            new Netmask(new byte[] { 255, 252, 0, 0 }, 64, 262142, NetmaskClass.A4),
            new Netmask(new byte[] { 255, 254, 0, 0 }, 128, 131070, NetmaskClass.A2),
            new Netmask(new byte[] { 255, 255, 0, 0 }, 256, 65534, NetmaskClass.B1),
            new Netmask(new byte[] { 255, 255, 128, 0 }, 2, 32766, NetmaskClass.B128),
            new Netmask(new byte[] { 255, 255, 192, 0 }, 4, 16382, NetmaskClass.B64),
            new Netmask(new byte[] { 255, 255, 224, 0 }, 8, 8190, NetmaskClass.B32),
            new Netmask(new byte[] { 255, 255, 240, 0 }, 16, 4094, NetmaskClass.B16),
            new Netmask(new byte[] { 255, 255, 248, 0 }, 32, 2046, NetmaskClass.B8),
            new Netmask(new byte[] { 255, 255, 252, 0 }, 64, 1022, NetmaskClass.B4),
            new Netmask(new byte[] { 255, 255, 254, 0 }, 128, 510, NetmaskClass.B2),
            new Netmask(new byte[] { 255, 255, 255, 128 }, 256, 254, NetmaskClass.C1),
            new Netmask(new byte[] { 255, 255, 255, 128 }, 2, 126, NetmaskClass.C1s2),
            new Netmask(new byte[] { 255, 255, 255, 192 }, 4, 62, NetmaskClass.C1s4),
            new Netmask(new byte[] { 255, 255, 255, 224 }, 8, 30, NetmaskClass.C1s8),
            new Netmask(new byte[] { 255, 255, 255, 240 }, 16, 14, NetmaskClass.C1s16),
            new Netmask(new byte[] { 255, 255, 255, 248 }, 32, 6, NetmaskClass.C1s32),
            new Netmask(new byte[] { 255, 255, 255, 252 }, 64, 2, NetmaskClass.C1s64),
            new Netmask(new byte[] { 255, 255, 255, 254 }, 128, 0, NetmaskClass.C1s128),
        };

        private static void InitializeDefaultMasks()
        {
            _defaultNetmasks.Add(IPAdressType.A, new Netmask(new byte[4] { 255, 0, 0, 0 }, 0, 0, NetmaskClass.A128));
            _defaultNetmasks.Add(IPAdressType.B, new Netmask(new byte[4] { 255, 255, 0, 0 }, 256, 65534, NetmaskClass.B1));
            _defaultNetmasks.Add(IPAdressType.C, new Netmask(new byte[4] { 255, 255, 255, 0 }, 256, 254, NetmaskClass.C1));
        }

        private static void InitializeMasksList()
        {
            for (int i = 0; i < _netmasks.Length; i++)
                _listNetmask.Add(i + 9, _netmasks[i]);

            _netmaskRange.Add(IPAdressType.A, new NetmaskIndexRange(9, 16));
            _netmaskRange.Add(IPAdressType.B, new NetmaskIndexRange(16, 24));
            _netmaskRange.Add(IPAdressType.C, new NetmaskIndexRange(24, 32));
        }
        public static Netmask GetDefaultMask(IPAdressType ipAdressType)
        {
            if (_defaultNetmasks.Count == 0)
                InitializeDefaultMasks();

            return _defaultNetmasks[ipAdressType];
        }

        public static Netmask GetRandomMask(IPAdressType ipAdressType)
        {
            if (_listNetmask.Count == 0)
                InitializeMasksList();

            int netmaskIndex = _netmaskRange[ipAdressType].GetRandomIndex();

            if (_listNetmask.TryGetValue(netmaskIndex, out Netmask netmask) == true)
                return netmask;

            throw new Exception($"Failed to get mask number {netmaskIndex}");
        }

        private class NetmaskIndexRange
        {
            private Random _random = new Random();

            private int _minimum;
            private int _maximum;

            public NetmaskIndexRange(int minimum, int maximum)
            {
                _minimum = minimum;
                _maximum = maximum;
            }

            public int GetRandomIndex() => _random.Next(_minimum, _maximum);
        }
            
    }
}
