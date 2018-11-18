using System;
using Disboard.Misskey.Enums;
using Disboard.Misskey.Extensions;
using System.Collections.Generic;

namespace Mobisskey.ViewModels
{
	public static class ReactionEx
	{
		public static void Add(this IList<ReactionViewModel> list, NoteViewModel note, Reaction reaction, Reaction? myReaction, long amount)
		{
			if (amount > 0)
			{
				var rvm = new ReactionViewModel(note, reaction)
				{
					Emoji = reaction.ToActualEmoji(),
					Reaction = reaction.ToStr(),
					Count = (int)amount,
					IsMyReaction = reaction == myReaction,
				};
				list.Add(rvm);
			}
		}
		public static string ToActualEmoji(this Reaction reaction)
		{
			switch (reaction)
			{
				case Reaction.Like:
					return "👍";
				case Reaction.Love:
					return "❤️";
				case Reaction.Laugh:
					return "😆";
				case Reaction.Hmm:
					return "🤔";
				case Reaction.Surprise:
					return "😮";
				case Reaction.Congrats:
					return "🎉";
				case Reaction.Angry:
					return "💢";
				case Reaction.Confused:
					return "😥";
				case Reaction.Rip:
					return "😇";
				case Reaction.Pudding:
					return "🍮";
				default:
					throw new ArgumentOutOfRangeException(nameof(reaction), reaction, null);
			}
		}
	}
}