namespace SmartFactory.Application.Features.Analytics.DTOs;

public class DailySummaryDto
{
    public int TotalProduced { get; set; }
    public int TotalFaults { get; set; }
    public double Efficiency { get; set; }
}

