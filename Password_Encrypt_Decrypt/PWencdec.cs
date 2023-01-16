using System.Security.Cryptography;
using System.Text;

namespace Password_Encrypt_Decrypt
{

	public static class PWencdec
	{
		public static string key = "asdfasfasdf";
		public static string encrypt(string password)
		{
			if (password == null)
			{
				return "Enter Password";
			}
			else
			{
				password += key;
				var passwordbytes = Encoding.UTF8.GetBytes(password);
				return Convert.ToBase64String(passwordbytes);
			}
		}

		public static string decrypt(string password) { 
		
		if (password == null)
			{
				return "Enter Password";
			}
			else
			{
				var base64encodebytes = Convert.FromBase64String(password);
				var result = Encoding.UTF8.GetString(base64encodebytes);
				result = result.Substring(0, result.Length - key.Length);
				return result;
			}
		}
	}
}
