﻿using System;
namespace DataConCore.Handels.HandelDto
{
	public class ConsulSetting : IAppCore
    {
		public string ConsulService { get; set; }

		public string Datacenter { get; set; }

		public string ServerName { get; set; }

		public string Ip { get; set; }

		public int Port { get; set; }

		public string HealthPath { get; set; }


        public string[] Tags { get; set; }
	}
}

