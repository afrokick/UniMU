using MUnique.OpenMU.Network.Xor;

namespace MUnique.OpenMU.Network
{
	using MUnique.OpenMU.Network.SimpleModulus;

	/// <summary>
	/// The default encryptor used by the server to encrypt outgoing data packets.
	/// It encrypts 0xC3 and 0xC4-packets with the "simple modulus" algorithm.
	/// </summary>
	public class Encryptor : ComposableEncryptor
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Encryptor"/> class
		/// which uses default keys.
		/// </summary>
		public Encryptor()
			: this(SimpleModulusEncryptor.DefaultClientKey)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Encryptor"/> class.
		/// </summary>
		/// <param name="encryptionKey">The encryption key.</param>
		public Encryptor(SimpleModulusKeys encryptionKey)
		{
			this.AddEncryptor(new Xor32Encryptor())
				.AddEncryptor(new SimpleModulusEncryptor(encryptionKey));

		}
	}
}
