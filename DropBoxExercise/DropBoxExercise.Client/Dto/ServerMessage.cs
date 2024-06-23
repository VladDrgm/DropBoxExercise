using DropBoxExercise.CommonUtils.Enums;

namespace DropBoxExercise.Client.Dto;

public record ServerMessageDto(ChangeType ChangeType, string ItemName, byte[]? ItemData, string? OldItemName, string ItemPath, string? OldItemPath);