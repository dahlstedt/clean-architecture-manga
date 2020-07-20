﻿namespace Domain.Accounts.Debits
{
    using System;
    using System.Threading.Tasks;
    using Services;
    using ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class DebitBuilder
    {
        private readonly IAccountFactory _accountFactory;
        private readonly Notification _notification;
        private readonly ICurrencyExchange _currencyExchange;

        private IAccount? _account;
        private Currency? _currency;
        private PositiveMoney? _positiveMoney;
        private DateTime? _transactionDate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountFactory"></param>
        /// <param name="notification"></param>
        /// <param name="currencyExchange"></param>
        public DebitBuilder(
            IAccountFactory accountFactory,
            Notification notification,
            ICurrencyExchange currencyExchange)
        {
            this._accountFactory = accountFactory;
            this._notification = notification;
            this._currencyExchange = currencyExchange;
        }

        public DebitBuilder Account(IAccount account)
        {
            this._account = account;
            return this;
        }

        public DebitBuilder Amount(decimal amount, string currencyCode)
        {
            this._currency = Currency.Create(this._notification, currencyCode);
            if (this._currency != null)
            {
                this._positiveMoney = PositiveMoney.Create(
                    this._notification,
                    amount,
                    this._currency.Value);
            }

            return this;
        }

        public DebitBuilder Timestamp()
        {
            this._transactionDate = DateTime.Now;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>IDebit</returns>
        public async Task<IDebit> Build()
        {
            if (this._account is AccountNull ||
                !this._currency.HasValue ||
                !this._positiveMoney.HasValue ||
                !this._transactionDate.HasValue ||
                !this._notification.IsValid)
            {
                return DebitNull.Instance;
            }

            if (!await this._currencyExchange
                .IsCurrencyAllowed(this._currency.Value)
                .ConfigureAwait(false))
            {
                return DebitNull.Instance;
            }

            return this.BuildInternal();
        }

        private IDebit BuildInternal() =>
            this._accountFactory.NewDebit(
                this._account!,
                this._positiveMoney!.Value,
                this._transactionDate!.Value);
    }
}