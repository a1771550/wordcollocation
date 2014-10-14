﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UI.Classes;

namespace UI.Helpers
{
	/// <summary>
	/// Contains static text methods.
	/// </summary>
	public static class TextHelper
	{
		/// <summary>
		/// Count occurrences of strings.
		/// </summary>
		public static int CountStringOccurrences(string text, string pattern)
		{
			// Loop through all instances of the string 'text'.
			int count = 0;
			int i = 0;
			while ((i = text.IndexOf(pattern, i, StringComparison.Ordinal)) != -1)
			{
				i += pattern.Length;
				count++;
			}
			return count;
		}

		/// <summary>
		/// Encrypt a string using dual encryption method. Return a encrypted cipher Text
		/// </summary>
		/// <param name="toEncrypt">string to be encrypted</param>
		/// <param name="useHashing">use hashing? send to for extra secirity</param>
		/// <returns></returns>
		public static string Encrypt(string toEncrypt, bool useHashing)
		{
			byte[] keyArray;
			byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

			// Get the key from config file
			string key = SiteConfiguration.SecurityKey;

			var salt = Guid.NewGuid().ToString();
			var path = Path.Combine(SiteConfiguration.KeyPath, SiteConfiguration.KeyFileName);
			StreamWriter writer = new StreamWriter(path, false);
			using (writer)
			{
				writer.WriteLine(salt);
			}
			key += salt;

			if (useHashing)
			{
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
				hashmd5.Clear();
			}
			else
				keyArray = Encoding.UTF8.GetBytes(key);

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
			{
				Key = keyArray,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			};

			ICryptoTransform cTransform = tdes.CreateEncryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			tdes.Clear();
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}
		/// <summary>
		/// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
		/// </summary>
		/// <param name="cipherString">encrypted string</param>
		/// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
		/// <returns></returns>
		public static string Decrypt(string cipherString, bool useHashing)
		{
			byte[] keyArray;
			byte[] toEncryptArray = Convert.FromBase64String(cipherString);

			//get the salt value
			var path = Path.Combine(SiteConfiguration.KeyPath, SiteConfiguration.KeyFileName);
			StreamReader reader = new StreamReader(path);
			string salt;
			using (reader)
			{
				salt = reader.ReadLine();
			}

			//Get your key from config file to open the lock!
			string key = SiteConfiguration.SecurityKey;
			key += salt;

			if (useHashing)
			{
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
				hashmd5.Clear();
			}
			else
				keyArray = Encoding.UTF8.GetBytes(key);

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
			{
				Key = keyArray,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			};

			ICryptoTransform cTransform = tdes.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

			tdes.Clear();
			return Encoding.UTF8.GetString(resultArray);
		}

		public static string HashMD5(string email)
		{
			MD5 md5 = MD5.Create();
			byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(email));
			StringBuilder builder = new StringBuilder();
			foreach (var d in data)
				builder.Append(d.ToString("x2"));
			return builder.ToString();
		}

		/// <summary>
		/// Uppercase first letters of all words in the string.
		/// </summary>
		public static string UpperFirst(string s)
		{
			return Regex.Replace(s, @"\b[a-z]\w+", delegate(Match match)
			{
				string v = match.ToString();
				return char.ToUpper(v[0]) + v.Substring(1);
			});
		}
	}
}