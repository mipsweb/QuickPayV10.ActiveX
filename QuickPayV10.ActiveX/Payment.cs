using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using QuickPayV10.ActiveX.Interface;

namespace QuickPayV10.ActiveX
{
	[ClassInterface(ClassInterfaceType.None), Guid("10F32360-0987-4189-A19E-3763120CB7CD"), ProgId("QuickPayV10.ActiveX.Payment")]
	public class Payment : IPayment
    {
		[ComVisible(true)]
		public Int32 MerchantId { get; set; }
		[ComVisible(true)]
		public Int32 AgreementId { get; set; }
		[ComVisible(true)]
		public string OrderId { get; set; }
		[ComVisible(true)]
		public Int32 Amount { get; set; }
		[ComVisible(true)]
		public string Currency { get; set; }
		[ComVisible(true)]
		public string ContinueUrl { get; set; }
		[ComVisible(true)]
		public string CancelUrl { get; set; }
		[ComVisible(true)]
		public string CallbackUrl { get; set; }
		[ComVisible(true)]
		public string Language { get; set; }
		[ComVisible(true)]
		public Int32 Autocapture { get; set; }
		[ComVisible(true)]
		public string PaymentMethods { get; set; }
		[ComVisible(true)]
		public string PublicKey { get; set; }

		public string ComputeChecksum()
		{
			Validate();

			var parameters = new Dictionary<string, string>();
			parameters.Add("version", "v10");
			parameters.Add("merchant_id", MerchantId.ToString());
			parameters.Add("agreement_id", AgreementId.ToString());
			parameters.Add("order_id", OrderId);
			parameters.Add("amount", Amount.ToString());
			parameters.Add("currency", Currency);
			parameters.Add("continue_url", ContinueUrl);
			parameters.Add("cancel_url", CancelUrl);
			parameters.Add("callback_url", CallbackUrl);
			parameters.Add("language", Language);

			if (Autocapture == 1)
			{
				parameters.Add("autocapture", Autocapture.ToString());
			}

			if (!string.IsNullOrEmpty(PaymentMethods))
			{
				parameters.Add("payment_methods", PaymentMethods);
			}

			var result = String.Join(" ", parameters.OrderBy(c => c.Key).Select(c => c.Value).ToArray());

			var e = Encoding.UTF8;

			var hmac = new HMACSHA256(e.GetBytes(PublicKey));
			byte[] b = hmac.ComputeHash(e.GetBytes(result));

			var s = new StringBuilder();
			for (int i = 0; i < b.Length; i++)
			{
				s.Append(b[i].ToString("x2"));
			}

			return s.ToString();
		}

		private void Validate()
		{
			if (MerchantId == 0)
			{
				throw new NullReferenceException(MerchantId.ToString());
			}

			if (AgreementId == 0)
			{
				throw new NullReferenceException(AgreementId.ToString());
			}

			if (string.IsNullOrEmpty(OrderId))
			{
				throw new NullReferenceException(OrderId);
			}

			if (Amount <= 0)
			{
				throw new Exception("Amount must be greater than 0");
			}

			if (string.IsNullOrEmpty(Currency))
			{
				throw new NullReferenceException(Currency);
			}

			if (string.IsNullOrEmpty(ContinueUrl))
			{
				throw new NullReferenceException(ContinueUrl);
			}

			if (string.IsNullOrEmpty(CancelUrl))
			{
				throw new NullReferenceException(CancelUrl);
			}

			if (string.IsNullOrEmpty(CallbackUrl))
			{
				throw new NullReferenceException(CallbackUrl);
			}

			if (string.IsNullOrEmpty(Language))
			{
				throw new NullReferenceException(Language);
			}

			if (!(Autocapture == 0 || Autocapture == 1))
			{
				throw new Exception("Autocapture must be 0 or 1");
			}

			if (string.IsNullOrEmpty(PublicKey))
			{
				throw new NullReferenceException(PublicKey);
			}
		}
	}
}
