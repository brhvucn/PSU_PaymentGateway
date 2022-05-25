﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PSU_PaymentGateway.Models;
using PSU_PaymentGateway.Repository;
using PSU_PaymentGateway.Services;
using System;
using System.Threading.Tasks;

namespace PSU_PaymentGateway.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IMemoryRepository transactionRepository;
        private IThrottleService throttleService;        
        public PaymentController(IMemoryRepository transactionRepository, IThrottleService throttleService)
        {
            this.throttleService = throttleService;
            this.transactionRepository = transactionRepository;            
        }

        [HttpPost]
        [Route("process")]
        public Result<Transaction> ProcessPayment(PaymentRequest request)
        {
            //check for throttling
            if(!this.throttleService.CanExecute())
            {
                return Result.Fail<Transaction>("The Payment service is not ready");
            }
            //simulate process
            Result<Payment> paymentResult = Payment.Create(request.CardNumber, request.ExpirationDate, request.CVC);
            if (paymentResult.IsSuccess)
            {
                Result<Transaction> transactionResult = Transaction.Create(request.Amount, paymentResult.Value);
                if (transactionResult.IsSuccess)
                {
                    Result result = this.transactionRepository.AddTransaction(transactionResult.Value);
                    if (result.IsFailure)
                    {
                        return Result.Fail<Transaction>(result.Error);
                    }
                }
                return transactionResult;
            }
            else
            {
                return Result.Fail<Transaction>(paymentResult.Error);
            }
        }
    }
}
