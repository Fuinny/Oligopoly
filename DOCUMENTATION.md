# Documentation
This documentation provides instructions on how to add new companies and events to the game.
To add your company or event, you will need to edit the appropriate .xml file in Data folder, which contain all the necessary data for the game.

Also, thank you for your interest in adding your own content to the game :P

## How to add your own company
1. Open Companies.xml file.
2. Find Company element.
3. Copy the following template and paste it below last Company element:
      ```xml
      <Company id="COMPANY_ID">
        <Name>COMPANY_NAME</Name>
        <Industry>COMPANY_INDUSTRY</Industry>
        <SharePrice>COMPANY_SHARE_PRICE</SharePrice>
        <Description>COMPANY_DESCRIPTION</Description>
      </Company>
      ```
4. Replace COMPANY_ID, COMPANY_NAME, COMPANY_INDUSTRY, COMPANY_SHARE_PRICE, and COMPANY_DESCRIPTION with the appropriate values for your new company.
5. Save the changes to the Companies.xml file.

> **Please note:**
> 
> **COMPANY_ID** - can be anything, make it so that it is easier for you to navigate in the xml file, **but** you need to use underscore between words. Company name is used by default, but you can use something else.
> 
> Example: ```ABC_Company_Id```
> 
> **COMPANY_NAME**, **COMPANY_INDUSTRY** and **COMPANY_DESCRIPTION** - can be anything, except null or whitespace.
> 
> Example: ```ABC Company Name```, ```ABC Company Industry``` and ```ABC Company long-long-long description.```
>
> **COMPANY_SHARE_PRICE** - can be any value greater than zero.

## How to add your own event
1. Open Events.xml file.
2. Find Event element.
3. Copy the following template and paste it below last Event element:
      ```xml
      <Event id="EVENT_ID">
        <Effect>EVENT_EFFECT</Effect>
        <Target>EVENT_TARGET</Target>
        <Title>EVENT_TITLE</Title>
        <Content>EVENT_CONTENT</Content>
      </Event>
      ```
4. Replace EVENT_ID, EVENT_EFFECT, EVENT_TARGET, EVENT_TITLE and EVENT_CONTENT with the appropriate values for your new event.
5. Save the changes to the Events.xml file.

> **Please note:**
>
> **EVENT_ID** - can be anything, make it so that it is easier for you to navigate in the xml file, **but** you need to use underscore between words.
> The default structure is: Company_Event, but you can use other structure.
>
> Example: ```ABC_Company_new_event```
>
> **EVENT_TARGET** - **must** match the name of the company to which the effect is to be applied. Obviously cannot be null or whitespace.
>
> Example: ```ABC Company Name```
>
> **EVENT_TITLE** and **EVENT_CONTENT** - can be anything, except null or whitespace.
>
> Example: ```ABC Company something happened``` and ```ABC Company long-long-long event content.```
