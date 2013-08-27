using System;
using NUnit.Framework;
using nTransit;

namespace nTransit {
	[TestFixture]
	public class StandardInputPortTests {
		[Test]
		public void InvalidOperationException_is_thrown_when_recieving_from_a_closed_port() {
			var port = new StandardInputPort();
			port.Connection = new Connection();
			port.Close();
			Assert.Throws<InvalidOperationException>(() => {
				port.Receive();
			});
		}

		[Test]
		public void InvalidOperationException_is_thrown_when_receiving_from_a_port_with_no_connection() {
			var port = new StandardInputPort();
			Assert.Throws<InvalidOperationException>(() => {
				port.Receive();
			});
		}

		[Test]
		public void Receiving_a_packet_causes_the_associated_component_to_claim_the_packet() {
			var port = new StandardInputPort();
			port.Connection = new Connection();
			var packet = new InformationPacket("Packet to be claimed");
			packet.Owner = port.Connection;
			port.Connection.SetInitialData(packet);
			var component = new MockComponent();
			port.Component = component;

			var ip = port.Receive();
			Assert.AreSame(component, packet.Owner);
			Assert.AreSame(component, ip.Owner);
		}
	}
}