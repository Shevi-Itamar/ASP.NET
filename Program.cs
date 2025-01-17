using lessson1.Services;
using lessson1.Interfaces;
using lessson1.MiddleWares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddJewelService();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  
              .AllowAnyMethod()  
              .AllowAnyHeader(); 
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAllOrigins");
app.UseCatchMiddleWare();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
