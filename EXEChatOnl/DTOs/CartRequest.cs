namespace EXEChatOnl.DTOs;

public class CartRequest
{
    public int? Id { get; set; }
    public int? ProductId {get;set;}
    
    public int? CustomerId {get;set;}
    
    public string? imageProduct {get; set;}
    
    public string? productName {get; set;}
    
    public decimal? productPrice {get; set;}
    
    public int quantity {get; set;}
    
    public decimal? totalPrice {get; set;}
    
}