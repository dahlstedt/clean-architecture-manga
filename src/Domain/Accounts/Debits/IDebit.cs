// <copyright file="IDebit.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Accounts.Debits
{
    using ValueObjects;

    /// <summary>
    ///     Debit.
    /// </summary>
    public interface IDebit
    {
        DebitId Id { get; }

        /// <summary>
        ///     Gets the Amount.
        /// </summary>
        PositiveMoney Amount { get; }
    }
}
