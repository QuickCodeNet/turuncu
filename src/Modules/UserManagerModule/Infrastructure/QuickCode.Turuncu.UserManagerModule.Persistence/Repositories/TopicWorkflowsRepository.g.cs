//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was generated by QuickCode. 
// Runtime Version:1.0
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using QuickCode.Turuncu.UserManagerModule.Application.Models;
using QuickCode.Turuncu.UserManagerModule.Domain.Entities;
using QuickCode.Turuncu.UserManagerModule.Application.Interfaces.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuickCode.Turuncu.UserManagerModule.Persistence.Contexts;
using QuickCode.Turuncu.UserManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.UserManagerModule.Persistence.Repositories
{
    public partial class TopicWorkflowsRepository : ITopicWorkflowsRepository
    {
        private readonly WriteDbContext _writeContext;
        private readonly ReadDbContext _readContext;
        private readonly ILogger<TopicWorkflowsRepository> _logger;
        public TopicWorkflowsRepository(ILogger<TopicWorkflowsRepository> logger, WriteDbContext writeContext, ReadDbContext readContext)
        {
            _writeContext = writeContext;
            _readContext = readContext;
            _logger = logger;
        }

        public async Task<DLResponse<TopicWorkflows>> InsertAsync(TopicWorkflows value)
        {
            var returnValue = new DLResponse<TopicWorkflows>(value, "Not Defined");
            try
            {
                await _writeContext.TopicWorkflows.AddAsync(value);
                await _writeContext.SaveChangesAsync();
                returnValue.Value = value;
            }
            catch (SqlException ex)
            {
                _logger.LogError("{repoName} SqlException {error}", "TopicWorkflows Insert", ex.Message);
                if (ex.Number.Equals(2627))
                {
                    returnValue.Code = 999;
                    returnValue.Value = value;
                }
                else
                {
                    returnValue.Code = 998;
                    returnValue.Value = value;
                }

                returnValue.Message = ex.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError("{repoName} Exception {error}", "TopicWorkflows Insert", ex.Message);
                returnValue.Code = 500;
                returnValue.Value = value;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<bool>> UpdateAsync(TopicWorkflows value)
        {
            var returnValue = new DLResponse<bool>(false, "Success");
            try
            {
                _writeContext.Set<TopicWorkflows>().Update(value);
                await _writeContext.SaveChangesAsync();
                returnValue.Value = true;
            }
            catch (SqlException ex)
            {
                _logger.LogError("{repoName} SqlException {error}", "TopicWorkflows Update", ex.Message);
                if (ex.Number.Equals(2627))
                {
                    returnValue.Code = 999;
                }
                else
                {
                    returnValue.Code = 998;
                }

                returnValue.Message = ex.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError("{repoName} Exception {error}", "TopicWorkflows", ex.Message);
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<bool>> DeleteAsync(TopicWorkflows value)
        {
            var returnValue = new DLResponse<bool>(false, "Success");
            try
            {
                _writeContext.TopicWorkflows.Remove(value);
                await _writeContext.SaveChangesAsync();
                returnValue.Value = true;
            }
            catch (SqlException ex)
            {
                _logger.LogError("{repoName} SqlException {error}", "TopicWorkflows Delete", ex.Message);
                if (ex.Number.Equals(2627))
                {
                    returnValue.Code = 999;
                }
                else
                {
                    returnValue.Code = 998;
                }

                returnValue.Message = ex.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError("{repoName} Exception {error}", "TopicWorkflows Delete", ex.Message);
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<TopicWorkflows>> GetByPkAsync(int id)
        {
            var returnValue = new DLResponse<TopicWorkflows>();
            try
            {
                var result =
                    from topic_workflows in _readContext.TopicWorkflows
                    where topic_workflows.Id.Equals(id)select topic_workflows;
                returnValue.Value = await result.FirstAsync();
                if (returnValue.Value == null)
                {
                    returnValue.Code = 404;
                    returnValue.Message = $"Not found in TopicWorkflows";
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{repoName} Exception {error}", "TopicWorkflows GetByPk", ex.Message);
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<List<TopicWorkflows>>> ListAsync(int? pageNumber = null, int? pageSize = null)
        {
            var returnValue = new DLResponse<List<TopicWorkflows>>();
            try
            {
                if (pageNumber < 1)
                {
                    returnValue.Code = 404;
                    returnValue.Message = "Page Number must be greater than 1";
                }
                else
                {
                    if (pageNumber != null)
                    {
                        var skip = ((pageNumber - 1) * pageSize);
                        var take = pageSize;
                        returnValue.Value = await _readContext.TopicWorkflows.Skip(skip.Value).Take(take.Value).ToListAsync();
                    }
                    else
                    {
                        returnValue.Value = await _readContext.TopicWorkflows.ToListAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<int>> CountAsync()
        {
            var returnValue = new DLResponse<int>();
            try
            {
                returnValue.Value = await _readContext.TopicWorkflows.CountAsync();
            }
            catch (Exception ex)
            {
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<List<TopicWorkflowsGetWorkflowsResponseDto>>> TopicWorkflowsGetWorkflowsAsync(int topicWorkflowsKafkaEventId)
        {
            var returnValue = new DLResponse<List<TopicWorkflowsGetWorkflowsResponseDto>>();
            try
            {
                var queryableResult =
                    from topic_workflows in _readContext.TopicWorkflows
                    where topic_workflows.KafkaEventId.Equals(topicWorkflowsKafkaEventId)select new TopicWorkflowsGetWorkflowsResponseDto()
                    {
                        Id = topic_workflows.Id,
                        KafkaEventId = topic_workflows.KafkaEventId,
                        WorkflowContent = topic_workflows.WorkflowContent
                    };
                var result = await queryableResult.ToListAsync();
                returnValue.Value = result;
            }
            catch (Exception ex)
            {
                _logger.LogError("{repoName} Exception {error}", "TopicWorkflows TopicWorkflowsGetWorkflows", ex.Message);
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }
    }
}