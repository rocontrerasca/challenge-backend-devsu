using Challenge.Devsu.Core.Enums;

namespace Challenge.Devsu.Application.DTOs;

public class MoveReportPdfResponseDto
{
    public string FileName { get; set; } = string.Empty;
    public string Base64 { get; set; } = string.Empty;
}
