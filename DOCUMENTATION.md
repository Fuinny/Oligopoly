# Documentation
The documentation contains instructions for adding new companies and events to the game. 
To add your company or event, you will need to edit the Data.xml file, which contains all the data used in the game.

## How to add your own company
1. Open Data.xml file
2. Find Companies element
3. Copy the following template and paste it below last Company element:
```xml
<Company id="COMPANY_ID">
  <Name>COMPANY_NAME</Name>
  <Ticker>COMPANY_TICKER</Ticker>
  <Industry>COMPANY_INDUSTRY</Industry>
  <SharePrice>COMPANY_SHARE_PRICE</SharePrice>
  <Description>COMPANY_DESCRIPTION</Description>
</Company>
```
4. Replace COMPANY_ID, COMPANY_NAME, COMPANY_TICKER, COMPANY_INDUSTRY, COMPANY_SHARE_PRICE, and COMPANY_DESCRIPTION with the appropriate values for your new company.
- Please note that:
  - Company id value can be anything, make it so that it is easier for you to navigate in the xml file.
  - Make sure the Ticker attribute value is unique and does not match any other company Ticker values in the file.
  - Industry and Description CANNOT be null or whitespace. 
  - SharePrice can be ONLY positive.
5. Save the changes to the XML file.

## How to add your own event
1. Open Data.xml file
2. Find Events element
3. Copy the following template and paste it below last Event element:
```xml
<Event id="EVENT_ID">
  <Effect>EVENT_EFFECT</Effect>
  <Target>EVENT_TARGET</Target>
  <Type>EVENT_TYPE</Type>
  <Title>EVENT_TITLE</Title>
  <Content>EVENT_CONTENT</Content>
</Company>
```
4.Replace EVENT_ID, EVENT_EFFECT, EVENT_TARGET, EVENT_TYPE, EVENT_TITLE and EVENT_CONTENT with the appropriate values for your new event.
- Please note that:
  - Event id value can be anything, make it so that it is easier for you to navigate in the xml file.
  - Effect can be ONLY positive.
  - Make sure that Target attribute matches the Ticker attribute of the company whose price is need to be changed.
  - Type attribute value can be ONLY Positive or Negative.
  - Title and Content CANNOT be null or whitespace.
5. Save the changes to the XML file.
