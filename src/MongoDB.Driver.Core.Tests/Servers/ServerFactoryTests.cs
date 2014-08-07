﻿/* Copyright 2013-2014 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Net;
using FluentAssertions;
using MongoDB.Driver.Core.Clusters;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.ConnectionPools;
using MongoDB.Driver.Core.Connections;
using MongoDB.Driver.Core.Events;
using MongoDB.Driver.Core.Servers;
using NSubstitute;
using NUnit.Framework;

namespace MongoDB.Driver.Core.Tests.Servers
{
    [TestFixture]
    public class ServerFactoryTests
    {
        private ClusterId _clusterId;
        private IConnectionPoolFactory _connectionPoolFactory;
        private EndPoint _endPoint;
        private IConnectionFactory _heartbeatConnectionFactory;
        private IServerListener _listener;
        private ServerSettings _settings;

        [SetUp]
        public void Setup()
        {
            _clusterId = new ClusterId();
            _connectionPoolFactory = Substitute.For<IConnectionPoolFactory>();
            _endPoint = new DnsEndPoint("localhost", 27017);
            _heartbeatConnectionFactory = Substitute.For<IConnectionFactory>();
            _listener = Substitute.For<IServerListener>();
            _settings = new ServerSettings();
        }

        [Test]
        public void Constructor_should_throw_when_settings_is_null()
        {
            Action act = () => new ServerFactory(null, _connectionPoolFactory, _heartbeatConnectionFactory, _listener);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Constructor_should_throw_when_connectionPoolFactory_is_null()
        {
            Action act = () => new ServerFactory(_settings, null, _heartbeatConnectionFactory, _listener);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Constructor_should_throw_when_heartbeatConnectionFactory_is_null()
        {
            Action act = () => new ServerFactory(_settings, _connectionPoolFactory, null, _listener);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Constructor_should_not_throw_when_listener_is_null()
        {
            Action act = () => new ServerFactory(_settings, _connectionPoolFactory, _heartbeatConnectionFactory, null);

            act.ShouldNotThrow();
        }

        [Test]
        public void CreateServer_should_throw_if_clusterId_is_null()
        {
            var subject = new ServerFactory(_settings, _connectionPoolFactory, _heartbeatConnectionFactory, _listener);

            Action act = () => subject.CreateServer(null, _endPoint);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void CreateServer_should_throw_if_endPoint_is_null()
        {
            var subject = new ServerFactory(_settings, _connectionPoolFactory, _heartbeatConnectionFactory, _listener);

            Action act = () => subject.CreateServer(_clusterId, null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void CreateServer_should_return_Server()
        {
            var subject = new ServerFactory(_settings, _connectionPoolFactory, _heartbeatConnectionFactory, _listener);

            var result = subject.CreateServer(_clusterId, _endPoint);

            result.Should().NotBeNull();
            result.Should().BeOfType<ClusterableServer>();
        }
    }
}