// <copyright file="ReportService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright> --------------------------------------------------------------------------------------------------------------------

namespace LoanShark.Service.SocialService.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Microsoft.Data.SqlClient;
    using LoanShark.EF.Repository.SocialRepository;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Interfaces;

    /// <summary>
    /// Provides services for managing and handling reports.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IUserService userService;
        private IRepository repository;
        //private List<Report> reports;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService"/> class.
        /// </summary>
        /// <param name="repository">The repository instance for data access.</param>
        /// <param name="userService">The user service instance for user-related operations.</param>
        public ReportService(IRepository repository, IUserService userService)
        {
            this.repository = repository;
            //this.reports = repository.GetReportsList();
            this.userService = userService;
        }

        /// <summary>
        /// Retrieves a report by its ID.
        /// </summary>
        /// <param name="id">The ID of the report.</param>
        /// <returns>The report if found; otherwise, null.</returns>
        public async Task<Report?> GetReportById(int id)
        {
            var reports = await this.repository.GetReportsList();

            return reports.Find(report => report.MessageID == id);
        }

        /// <summary>
        /// Adds a new report to the list.
        /// </summary>
        /// <param name="report">The report to add.</param>
        public async Task AddReport(Report report)
        {
            var reports = await this.repository.GetReportsList();

            reports.Add(report);
        }

        /// <summary>
        /// Checks if a report exists for a specific message and reporter.
        /// </summary>
        /// <param name="messageID">The ID of the message being reported.</param>
        /// <param name="reporterUserID">The ID of the user reporting the message.</param>
        /// <returns>True if the report exists; otherwise, false.</returns>
        public async Task<bool> CheckIfReportExists(int messageID, int reporterUserID)
        {
            var reports = await this.repository.GetReportsList();

            return reports.Exists(report => report.MessageID == messageID && report.ReporterUserID == reporterUserID);
        }

        /// <summary>
        /// Increases the report count for a specific user.
        /// </summary>
        /// <param name="reportedID">The ID of the user being reported.</param>
        public async Task IncreaseReportCount(int reportedID)
        {
            User user = await this.userService.GetUserById(reportedID);

            if (user == null)
            {
                return;
            }

            user.IncreaseReportCount();
        }

        /// <summary>
        /// Logs a list of reported messages to the repository.
        /// </summary>
        /// <param name="reports">The list of reports to log.</param>
        public async Task LogReportedMessages(List<Report> reports)
        {
            foreach (var report in reports)
            {
                await this.repository.AddReport(report.MessageID, report.Reason, report.Description, report.Status);
            }
        }

        /// <summary>
        /// Sends a report for further processing.
        /// </summary>
        /// <param name="report">The report to send.</param>
        public async Task SendReport(Report report)
        {
            // todo - implement this method
        }
    }
}