﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asio
{
    class Channel : Audio.Channel
    {
        private int index;
        private string name;
        private ASIOSampleType type;
        public int Index { get { return index; } }
        public override string Name { get { return name; } }
        public ASIOSampleType Type { get { return type; } }

        public Channel(ASIOChannelInfo Info)
        {
            index = Info.channel;
            name = Info.name;
            type = Info.type;
        }

        public override string ToString()
        {
            return name + " " + Enum.GetName(typeof(ASIOSampleType), type);
        }
    }

    class Device : Audio.Device
    {
        private AsioObject instance;

        public Device(AsioObject Instance)
            : base(Instance.DriverName) 
        { 
            instance = Instance;
            instance.Init(IntPtr.Zero);
            inputs = instance.InputChannels.Select(i => new Asio.Channel(i)).ToArray();
            outputs = instance.OutputChannels.Select(i => new Asio.Channel(i)).ToArray();
        }

        public override Audio.Stream Open(Audio.Stream.SampleHandler Callback, Audio.Channel[] Input, Audio.Channel[] Output)
        {
            return new Stream(
                instance,
                Callback,
                Input.Cast<Channel>().ToArray(),
                Output.Cast<Channel>().ToArray());
        }

        public override void ShowControlPanel() { instance.ShowControlPanel(); }
    }
}
