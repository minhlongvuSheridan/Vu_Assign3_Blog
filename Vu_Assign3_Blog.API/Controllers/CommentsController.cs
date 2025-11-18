using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Vu_Assign3_Blog.Core.Interfaces;

namespace Vu_Assign3_Blog.API.Controllers;
[Route("api/[controller]")]
public class CommentsController
{
    private readonly ICommentRepository _repository; 
    public CommentsController(ICommentRepository repository) 
    { 
        _repository = repository; 
    } 
}
