// <copyright file="IReportService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoanShark.Service.SocialService.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LoanShark.EF.Repository.SocialRepository;
    using LoanShark.Domain;

    /// <summary>
    /// Provides methods for managing and handling reports.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Retrieves a report by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the report.</param>
        /// <returns>The report if found; otherwise, null.</returns>
        Task<Report?> GetReportById(int id);

        /// <summary>
        /// Adds a new report to the system.
        /// </summary>
        /// <param name="report">The report to add.</param>
        Task AddReport(Report report);

        /// <summary>
        /// Checks if a report exists for a specific message and reporter.
        /// </summary>
        /// <param name="messageID">The ID of the message being reported.</param>
        /// <param name="reporterUserID">The ID of the user reporting the message.</param>
        /// <returns>True if the report exists; otherwise, false.</returns>
        Task<bool> CheckIfReportExists(int messageID, int reporterUserID);

        /// <summary>
        /// Increases the report count for a specific reported entity.
        /// </summary>
        /// <param name="reportedID">The ID of the reported entity.</param>
        Task IncreaseReportCount(int reportedID);

        /// <summary>
        /// Logs a list of reported messages.
        /// </summary>
        /// <param name="reports">The list of reports to log.</param>
        Task LogReportedMessages(List<Report> reports);

        /// <summary>
        /// Sends a report for further processing.
        /// </summary>
        /// <param name="report">The report to send.</param>
        Task SendReport(Report report);
    }
}
