using System;
using System.Runtime.InteropServices;

namespace QuickPayV10.ActiveX.Interface
{
	[InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("FCDADF3B-16E5-4ABA-9A57-3D1701D50BF9")]
	public interface IPayment
	{
		Int32 MerchantId { get; set; }
		Int32 AgreementId { get; set; }
		String OrderId { get; set; }
		Int32 Amount { get; set; }
		String Currency { get; set; }
		String ContinueUrl { get; set; }
		String CancelUrl { get; set; }
		String CallbackUrl { get; set; }
		String Language { get; set; }
		Int32 Autocapture { get; set; }
		String PaymentMethods { get; set; }
		String PublicKey { get; set; }

		String ComputeChecksum();
	}
}
