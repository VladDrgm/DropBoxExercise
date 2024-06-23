using DropBoxExercise.CommonUtils.Enums;

namespace DropBoxExercise.Server.Dto;

public record ClientMessageDto(ChangeType ChangeType, string ItemName, byte[]? ItemData, string? OldItemName, string ItemPath, string OldItemPath);
