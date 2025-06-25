using System;
using System.ComponentModel.DataAnnotations;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers.JsonModels;
using API.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class MessagesController : BaseApiController
{
    private readonly IMessageRepository _messageRepo;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;

    public MessagesController(IMessageRepository messageRepository, IMapper mapper, IUserRepository userRepository)
    {
        _messageRepo = messageRepository;
        _mapper = mapper;
        _userRepo = userRepository;
    }

    [HttpPost()]
    public async Task<ActionResult> CreateMessage([FromBody, Required] CreateMessageDTO messageDTO)
    {
        var userName = User.GetUserName()!;
        if (string.Equals(userName, messageDTO.RecipientUsername, StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Failed to Send Message");
        }

        var sender = await _userRepo.GetUserByNameAsync(userName);
        var recipient = await _userRepo.GetUserByNameAsync(messageDTO.RecipientUsername);

        if (sender == null || recipient == null)
        {
            return BadRequest("Cannot send message, invalid participants");
        }

        var message = new Message()
        {
            Sender = sender,
            Recipient = recipient,
            Content = messageDTO.Content,
            SenderName = sender.UserName!,
            RecipientName = recipient.UserName!
        };

        await _messageRepo.AddMessage(message);
        if (!await _messageRepo.SaveAllAynsc())
        {
            return BadRequest("Failed to process message");
        }
        return Ok(_mapper.Map<MessageDTO>(message));
    }

    [HttpGet()]
    public async Task<ActionResult> GetUserMessages([FromQuery] MessageParams messageParams)
    {
        var userName = User.GetUserName()!;
        var userID = User.GetUserId();

        messageParams.UserID = Convert.ToInt32(userID);
        messageParams.UserName = userName;
        var messages = await _messageRepo.GetUserMessages(messageParams);
        return Ok(messages);
    }

     [HttpGet("thread/{recipient}")]
    public async Task<ActionResult> GetUserThreadByRecipient(string recipient)
    {
        var userName = User.GetUserName()!;
        var userID = User.GetUserId();
        var messages = await _messageRepo.GetMessageThread(Convert.ToInt32(userID), recipient);
        return Ok(messages);
    }

}
