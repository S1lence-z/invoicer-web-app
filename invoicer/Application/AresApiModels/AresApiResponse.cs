using Domain.Interfaces;

namespace Application.AresApiModels
{
	public class SubjectInformation : IAresApiResponse
	{
		public string? ico { get; set; }
		public string? obchodniJmeno { get; set; }
		public Sidlo? sidlo { get; set; }
		public string? pravniForma { get; set; }
		public string? financniUrad { get; set; }
		public string? datumVzniku { get; set; }
		public string? datumAktualizace { get; set; }
		public string? dic { get; set; }
		public string? icoId { get; set; }
		public AdresaDorucovaci? adresaDorucovaci { get; set; }
		public SeznamRegistraci? seznamRegistraci { get; set; }
		public string? primarniZdroj { get; set; }
		public IReadOnlyList<DalsiUdaje>? dalsiUdaje { get; set; }
		public IReadOnlyList<string>? czNace { get; set; }
	}

	public class Sidlo
	{
		public string? kodStatu { get; set; }
		public string? nazevStatu { get; set; }
		public int kodKraje { get; set; }
		public string? nazevKraje { get; set; }
		public int kodOkresu { get; set; }
		public int kodObce { get; set; }
		public string? nazevObce { get; set; }
		public int kodMestskehoObvodu { get; set; }
		public string? nazevMestskehoObvodu { get; set; }
		public int kodMestskeCastiObvodu { get; set; }
		public int kodUlice { get; set; }
		public string? nazevMestskeCastiObvodu { get; set; }
		public string? nazevUlice { get; set; }
		public int cisloDomovni { get; set; }
		public int kodCastiObce { get; set; }
		public int cisloOrientacni { get; set; }
		public string? nazevCastiObce { get; set; }
		public int kodAdresnihoMista { get; set; }
		public int psc { get; set; }
		public string? textovaAdresa { get; set; }
		public bool standardizaceAdresy { get; set; }
		public int typCisloDomovni { get; set; }
	}

	public class AdresaDorucovaci
	{
		public string? radekAdresy1 { get; set; }
		public string? radekAdresy2 { get; set; }
		public string? radekAdresy3 { get; set; }
	}

	public class SeznamRegistraci
	{
		public string? stavZdrojeVr { get; set; }
		public string? stavZdrojeRes { get; set; }
		public string? stavZdrojeRzp { get; set; }
		public string? stavZdrojeNrpzs { get; set; }
		public string? stavZdrojeRpsh { get; set; }
		public string? stavZdrojeRcns { get; set; }
		public string? stavZdrojeSzr { get; set; }
		public string? stavZdrojeDph { get; set; }
		public string? stavZdrojeSd { get; set; }
		public string? stavZdrojeIr { get; set; }
		public string? stavZdrojeCeu { get; set; }
		public string? stavZdrojeRs { get; set; }
		public string? stavZdrojeRed { get; set; }
	}

	public class DalsiUdaje
	{
		public IReadOnlyList<ObchodniJmeno>? obchodniJmeno { get; set; }
		public IReadOnlyList<Sidlo2>? sidlo { get; set; }
		public string? pravniForma { get; set; }
		public string? spisovaZnacka { get; set; }
		public string? datovyZdroj { get; set; }
	}

	public class Sidlo2
	{
		public Sidlo3? sidlo { get; set; }
		public bool primarniZaznam { get; set; }
	}

	public class Sidlo3
	{
		public string? kodStatu { get; set; }
		public string? nazevStatu { get; set; }
		public int kodKraje { get; set; }
		public string? nazevKraje { get; set; }
		public int kodOkresu { get; set; }
		public int kodObce { get; set; }
		public string? nazevObce { get; set; }
		public int kodSpravnihoObvodu { get; set; }
		public string? nazevSpravnihoObvodu { get; set; }
		public int kodMestskehoObvodu { get; set; }
		public string? nazevMestskehoObvodu { get; set; }
		public int kodMestskeCastiObvodu { get; set; }
		public int kodUlice { get; set; }
		public string? nazevMestskeCastiObvodu { get; set; }
		public string? nazevUlice { get; set; }
		public int cisloDomovni { get; set; }
		public int kodCastiObce { get; set; }
		public int cisloOrientacni { get; set; }
		public string? nazevCastiObce { get; set; }
		public int kodAdresnihoMista { get; set; }
		public int psc { get; set; }
		public string? textovaAdresa { get; set; }
		public bool standardizaceAdresy { get; set; }
		public int typCisloDomovni { get; set; }
	}

	public class ObchodniJmeno
	{
		public string? obchodniJmeno { get; set; }
		public bool primarniZaznam { get; set; }
	}
}
