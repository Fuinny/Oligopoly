# :question: How to add your own company
1. Open ```Companies.xml``` file.
2. Find ```Company``` element.
3. Copy the following template and paste it below last ```Company``` element:
   
      ```xml
      <Company id="COMPANY_ID">
        <Name>COMPANY_NAME</Name>
        <Industry>COMPANY_INDUSTRY</Industry>
        <SharePrice>COMPANY_SHARE_PRICE</SharePrice>
        <Description>COMPANY_DESCRIPTION</Description>
      </Company>
      ```

5. Replace **COMPANY_ID**, **COMPANY_NAME**, **COMPANY_INDUSTRY**, **COMPANY_SHARE_PRICE**, and **COMPANY_DESCRIPTION** with the appropriate values for your new company.
6. Save the changes to the ```Companies.xml``` file.

> **Please note:**
> 
> **COMPANY_ID** - can be anything, make it so that it is easier for you to navigate in the ```.xml``` file, **but** you need to use underscore between words. Company name is used by default, but you can use something else.
> 
> Example: ```ABC_Company_Id```
> 
> **COMPANY_NAME**, **COMPANY_INDUSTRY** and **COMPANY_DESCRIPTION** - can be anything, except null or whitespace.
> 
> Example: ```ABC Company Name```, ```ABC Company Industry``` and ```ABC Company long-long-long description.```
>
> **COMPANY_SHARE_PRICE** - can be any value greater than zero.