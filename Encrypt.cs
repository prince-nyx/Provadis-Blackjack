using System.Security.Cryptography;
using System.Text;

public class Encrypt
{

	public string GetHashedPassword(string Password)
	{
		byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(Password);
		byte[] passwordBytes = Encoding.UTF8.GetBytes("test");

		passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
		byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);
		string encryptedResult = Convert.ToBase64String(bytesEncrypted);
		return encryptedResult;
	}

	private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
	{
		byte[] encryptedBytes = null;
		byte[] saltBytes = new byte[] { 2, 1, 7, 3, 6, 4, 8, 5 };

		using (MemoryStream ms = new MemoryStream())
		{
			using (RijndaelManaged AES = new RijndaelManaged())
			{
				AES.KeySize = 256;
				AES.BlockSize = 128;

				var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
				AES.Key = key.GetBytes(AES.KeySize / 8);
				AES.IV = key.GetBytes(AES.BlockSize / 8);

				AES.Mode = CipherMode.CBC;

				using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
				{
					cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
					cs.Close();
				}
				encryptedBytes = ms.ToArray();
			}
		}

		return encryptedBytes;
	}
}