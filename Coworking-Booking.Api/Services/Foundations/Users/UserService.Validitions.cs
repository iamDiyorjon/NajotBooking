// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

namespace Coworking_Booking.Api.Services.Foundations.Users
{
    public partial class UserService
    {
		private void ValidateUser(User user)
		{
			ValidateUserNotNull(user);

			Validate(
				(Rule: IsInvalid(user.Id), Parameter: nameof(User.Id)),
				(Rule: IsInvalid(user.FullName), Parameter: nameof(User.FullName)),
				(Rule: IsInvalid(user.PhoneNumber), Parameter: nameof(User.PhoneNumber)),
		}

		private void ValidateUserId(Guid userId) =>
			Validate((Rule: IsInvalid(userId), Parameter: nameof(User.Id)));

		private void ValidateStorageUser(User maybeUser, Guid userId)
		{
			if (maybeUser is null)
			{
				throw new NotFoundUserException(userId);
			}
		}

		private void ValidateUserId(Guid userId) =>
			Validate((Rule: IsInvalid(userId), Parameter: nameof(User.Id)));

		private void ValidateStorageUser(User maybeUser, Guid userId)
		{
			if (maybeUser is null)
			{
				throw new NotFoundUserException(userId);
			}
		}

		private static dynamic IsInvalid(Guid id) => new
		{
			Condition = id == default,
			Message = "Id is required"
		};

		private static dynamic IsInvalid(string text) => new
		{
			Condition = string.IsNullOrWhiteSpace(text),
			Message = "Text is required"
		};

		private static void ValidateUserNotNull(User user)
		{
			if (user is null)
			{
				throw new NullUserException();
			}
		}

		private static void Validate(params (dynamic Rule, string Parameter)[] validations)
		{
			var invalidUserException = new InvalidUserException();

			foreach ((dynamic rule, string parameter) in validations)
			{
				if (rule.Condition)
				{
					invalidUserException.UpsertDataList(
						key: parameter,
						value: rule.Message);
				}
			}

			invalidUserException.ThrowIfContainsErrors();
		}
	}
}