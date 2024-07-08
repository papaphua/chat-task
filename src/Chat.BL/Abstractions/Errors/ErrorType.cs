﻿namespace Chat.BL.Abstractions.Errors;

public enum ErrorType
{
    Validation = 400,
    NotFound = 404,
    Conflict = 409,
    Internal = 500
}