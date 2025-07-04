﻿using Shared.Enums;

namespace Application.DTOs
{
	public record class NumberingSchemeDto
	{
		public static readonly string DefaultSeparator = "-";
		public int Id { get; set; }
		public string Prefix { get; set; } = string.Empty;
		public bool UseSeperator { get; set; } = true;
		public string Seperator { get; set; } = DefaultSeparator;
		public Position SequencePosition { get; set; } = Position.Start;
		public int SequencePadding { get; set; } = 3;
		public YearFormat YearFormat { get; set; } = YearFormat.FourDigit;
		public bool IncludeMonth { get; set; } = true;
		public ResetFrequency ResetFrequency { get; set; } = ResetFrequency.Yearly;
		public bool IsDefault { get; set; } = false;
	}
}
